using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cabinet_Interaction : MonoBehaviour
{
    [Tooltip("The state of the door")]
    public bool open;
    [Tooltip("If it is true, the door won't open")]
    public bool locked;
    [Tooltip("The maximum distance in which you can interact with the door")]
    public float maxDistance = 2;
    [Tooltip("The angle in which the door rotates")]
    public float movementPathLength = 1;
    [Tooltip("The speed in which the door moves")]
    public float speed = 1;
    public enum axis { X = 0, Y = 1, Z = 2 }
    [Tooltip("The axis of rotation of the door. Defualt is Y")]
    public axis directionAxis = axis.Y;
    [Tooltip("The key you have to press in order to open/close the door (use the 'Naming convention' of Unity).")]
    public string keyToPress = "e";
    [Tooltip("The text which will be shown when you look at this door")]
    public string textShow = "Press E to open the cabinet";

    private Vector3 originPosition;
    private Vector3 openPosition;
    private AudioSource sound;
    private float r;
    private bool opening;

    private Ray ray;
    private RaycastHit hit;
    private LayerMask layer;

    public Text textUI;
    private bool textShown;

    void Start()
    {
        originPosition = transform.localPosition;
        openPosition = originPosition;
        if (directionAxis == axis.Y) {
            openPosition = new Vector3(originPosition.x, originPosition.y + movementPathLength, originPosition.z);
        } else if (directionAxis == axis.X) {
            openPosition = new Vector3(originPosition.x + movementPathLength, originPosition.y, originPosition.z);
        } else if (directionAxis == axis.Z) {
            openPosition = new Vector3(originPosition.x, originPosition.y, originPosition.z + movementPathLength);
        }
        r = 0;
        textShown = false;
        sound = GetComponent<AudioSource>();
        layer = (1 << 0);
    }

    void Update()
    {
        ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        Debug.DrawRay(ray.origin, ray.direction, Color.red);
        r += Time.deltaTime * speed;
        if (Physics.Raycast(ray, out hit, maxDistance, layer) && hit.transform == this.transform) {
            if (textShown == false) {
                textShown = true;
                if (textUI != null) {
                    textUI.text = textShow;
                }
            }
            if (Input.GetKeyDown(keyToPress) && opening == false && locked == false) {
                open = !open;
                r = 0;
                opening = true;
                if (sound.clip != null) {
                    sound.Play();
                }
            }
        } else {
            if (textShown == true) {
                textShown = false;
                if (textUI != null) {
                    textUI.text = "";
                }
            }
        }
        if (open == false) {
            if (Vector3.Distance(originPosition, transform.localPosition) > 0.001F) {
                transform.localPosition = Vector3.Lerp(transform.localPosition, originPosition, r);
            } else {
                transform.localPosition = originPosition;
                r = 0;
                opening = false;
            }
        } else {
            if (Vector3.Distance(openPosition, transform.localPosition) > 0.001F) {
                transform.localPosition = Vector3.Lerp(transform.localPosition, openPosition, r);
            } else {
                transform.localPosition = openPosition;
                r = 0;
                opening = false;
            }
        }
    }
}
