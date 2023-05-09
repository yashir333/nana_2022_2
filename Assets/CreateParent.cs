using UnityEditor;
using UnityEngine;

public class CreateParent : ScriptableObject
{
    [MenuItem("Tools/Create Parent")]
    static void DoCreateParent()
    {
        // Get selected GameObjects
        GameObject[] selectedObjects = Selection.gameObjects;

        foreach (GameObject selectedObject in selectedObjects)
        {
            // Create parent GameObject
            GameObject parentObject = new GameObject(selectedObject.name + "Parent");

            // Set parent's transform values to child's transform values
            parentObject.transform.position = selectedObject.transform.position;
            parentObject.transform.rotation = selectedObject.transform.rotation;
            parentObject.transform.localScale = selectedObject.transform.localScale;

            // Make child a child of the parent
            selectedObject.transform.parent = parentObject.transform;

            // Reset child's transform values
            selectedObject.transform.localPosition = Vector3.zero;
            selectedObject.transform.localRotation = Quaternion.identity;
            selectedObject.transform.localScale = Vector3.one;
        }
    }
}
