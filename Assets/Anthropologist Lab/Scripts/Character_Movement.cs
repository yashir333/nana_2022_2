using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Movement : MonoBehaviour
{
    private Transform cam;
    public float speedMovement = 1;
    public float speedCamera = 1;

    private float rotationX;
    private float rotationY;

    public float minimumX = -360F;
    public float maximumX = 360F;
    public float minimumY = -75F;
    public float maximumY = 75F;

    private CharacterController controller;
    private Vector3 movement;
    private Quaternion originalRotation;
    private Quaternion camOriginalRotation;
    private float Y;
    private bool grounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
        rotationX = 0.0F;
        rotationY = 0.0F;
        originalRotation = transform.rotation;
        camOriginalRotation = cam.localRotation;
    }

    void Update()
    {
         rotationX += Input.GetAxis("Mouse X") * Time.deltaTime * speedCamera * 10;
         rotationY += Input.GetAxis("Mouse Y") * Time.deltaTime * speedCamera * 10;
        rotationX = ClampAngle(rotationX, minimumX, maximumX);
        rotationY = ClampAngle(rotationY, minimumY, maximumY);
        cam.localRotation =  Quaternion.Slerp( cam.localRotation, Quaternion.Euler(new Vector3(-rotationY, camOriginalRotation.eulerAngles.y, camOriginalRotation.eulerAngles.z)), Time.deltaTime * speedCamera);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3(0, originalRotation.eulerAngles.y + rotationX, 0)), Time.deltaTime * speedCamera);

        Vector3 boxOrigin = new Vector3(transform.position.x, transform.position.y - (controller.height / 2) + controller.radius, transform.position.z);
        RaycastHit hit;
        if (Physics.SphereCast(boxOrigin, controller.radius, -Vector3.up, out hit, 0.1F)) {
            grounded = true;
        } else {
            grounded = false;
        }

        movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        movement = transform.TransformDirection(movement);
        if (grounded == false) {
            Y += Time.deltaTime;
            movement.y = -9.81F * Y * Y;
        } else {
            movement.y = -9.81F;
            Y = 0;
        }
        controller.Move(movement * Time.deltaTime * speedMovement);
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        angle = angle % 360;
        if (angle < -180) {
            angle += 360;
        }
        if (angle > 180) {
            angle -= 360;
        }
        return Mathf.Clamp(angle, min, max);
    }

}
