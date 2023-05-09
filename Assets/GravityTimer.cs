using UnityEngine;

public class GravityTimer : MonoBehaviour
{
    private float timer = 3.0f; // Number of seconds before gravity is enabled
    private Rigidbody rb;
    private bool timerStarted = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!timerStarted && rb != null && rb.gameObject.activeInHierarchy)
        {
            timerStarted = true;
            Invoke("EnableGravity", timer); // Invoke the EnableGravity method after the specified time
        }
    }

    private void EnableGravity()
    {
        rb.useGravity = true; // Set the Use Gravity boolean parameter to true
    }
}




