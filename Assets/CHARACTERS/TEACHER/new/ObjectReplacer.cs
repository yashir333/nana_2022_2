using UnityEngine;

public class ObjectReplacer : MonoBehaviour
{
    public GameObject replacementObject; // the object to be instantiated
    private bool isActive = true;

    private void OnDisable()
    {
        if (isActive)
     
        {
            if (replacementObject != null)
            {
                //  Instantiate(replacementObject, transform.position, transform.rotation); // instantiate the replacement object at the same position and rotation as the original
            }
        
        }
    }

    private void OnEnable()
    {
        isActive = true;
    }

    private void OnDestroy()
    {
        isActive = false;
    }
}
