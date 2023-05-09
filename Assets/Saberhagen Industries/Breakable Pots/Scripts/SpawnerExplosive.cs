using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerExplosive : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera sceneCamera;
    [SerializeField] private GameObject explosivePrefab;

    void Start()
    {
        if (sceneCamera == null) sceneCamera = Camera.main;

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (sceneCamera == null) return;

            Vector3 spawnPosition;
            RaycastHit hit;

            Ray ray = sceneCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                spawnPosition = hit.point + new Vector3(0, 6f, 0);
                GameObject spawnEffect = Instantiate(explosivePrefab, this.transform);
                spawnEffect.transform.position = spawnPosition; 

            }



        }
    }
}
