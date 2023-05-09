using UnityEngine;


namespace SaberhagenIndustries
{
    public class ControllerCamera : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject cameraYaw;
        [SerializeField] private GameObject cameraPitch;
        [SerializeField] private GameObject cameraArm;

        [Header("Sensitivity")]
        [SerializeField] private float YawSensitivity = 300f;
        [SerializeField] private float PitchSensitivity = 200f;
        [SerializeField] private float ZoomSensitivity = 6f;

        private float yawRotation;
        private float pitchRotation;
        private float zoomDistance;

        
        private void Start()
        {
            yawRotation = cameraYaw.transform.localEulerAngles.y;
            pitchRotation = cameraPitch.transform.localEulerAngles.x;
            zoomDistance = cameraArm.transform.localPosition.z;
        }


        private void Update()
        {
            if (Input.GetMouseButtonDown(1)) Cursor.visible = false;
            if (Input.GetMouseButtonUp(1)) Cursor.visible = true;

            if (Input.GetMouseButton(1))
            {


                yawRotation += Input.GetAxisRaw("Mouse X") * YawSensitivity * Time.deltaTime;
                pitchRotation -= Input.GetAxisRaw("Mouse Y") * PitchSensitivity * Time.deltaTime;

                pitchRotation = Mathf.Clamp(pitchRotation, 10f, 90f); 

                cameraYaw.transform.localEulerAngles = new Vector3( 0, yawRotation, 0 );
                cameraPitch.transform.localEulerAngles = new Vector3( pitchRotation, 0, 0 );
            }

            zoomDistance += Input.GetAxisRaw("Mouse ScrollWheel") * ZoomSensitivity;
            zoomDistance = Mathf.Clamp(zoomDistance, -20f, -5f);

            cameraArm.transform.localPosition = new Vector3( 0, 0, zoomDistance );
        }


    }
}

