using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    public string targetTag;
    public GameObject childObject;

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collision involves a GameObject with the target tag
        if (collision.gameObject.CompareTag(targetTag))
        {
            // Activate the child object
            childObject.SetActive(true);

            // Set the position of the child object to the position of the tag object
            childObject.transform.position = collision.gameObject.transform.position;
        }
    }
}
