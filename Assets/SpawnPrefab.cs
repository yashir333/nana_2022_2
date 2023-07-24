using UnityEngine;

public class SpawnPrefab : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabs; // List of prefabs to randomly choose from

    private void Start()
    {
        SpawnRandomPrefab();
    }

    private void SpawnRandomPrefab()
    {
        if (prefabs.Length == 0)
        {
            Debug.LogWarning("No prefabs assigned!");
            return;
        }

        int randomIndex = Random.Range(0, prefabs.Length); // Generate a random index within the range of the prefabs array
        GameObject prefab = prefabs[randomIndex];

        GameObject newObject = Instantiate(prefab, transform.position, Quaternion.identity);
        newObject.transform.parent = transform;
    }
}
