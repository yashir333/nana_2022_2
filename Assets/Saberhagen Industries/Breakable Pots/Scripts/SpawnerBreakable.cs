using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

namespace SaberhagenIndustries
{
    public class SpawnerBreakable : MonoBehaviour
    {
        [Header("Spawning")]
        
        [SerializeField] private GameObject[] breakblePrefabs;
        [SerializeField] private bool autoRefill = true;
        [SerializeField] private int breakablesToSpawn = 30;

        [Header("Effects")]
        [SerializeField] private bool spawnEffect = true;
        [SerializeField] private GameObject spawnEffectPrefab;

        [Header("Placement")]
        [SerializeField] private bool checkSpawnForOverlap = true;
        [SerializeField] private float assetRadius = 0.5f;
        [SerializeField] private float spawnRadius = 4f;
        [SerializeField] private float hightOffset = 0f;

        [Header("Randomize")]
        [SerializeField] private bool randomizeMaterial = true;
        [SerializeField]
        private Vector2 randomHueRange = new Vector2(0F, 0F);
        [SerializeField]
        private Vector2 randomSaturationRange = new Vector2(1f, 1f);
        [SerializeField]
        private Vector2 randomLightnessRange = new Vector2(1f, 1f);


        private IEnumerator Start()
        {
            yield return StartCoroutine(RespawnBreakables());
            StartCoroutine(CheckBreakablesCount());
        }


        private IEnumerator CheckBreakablesCount()
        {
            WaitForSeconds waitCycle = new WaitForSeconds(0.5f);
            Breakable[] potsInScene = new Breakable[0];
            while (autoRefill)
            {
                yield return waitCycle;
                potsInScene = FindObjectsOfType<Breakable>();
                if (potsInScene.Length < breakablesToSpawn)
                {
                    yield return StartCoroutine(RespawnBreakables());
                }
            }
        }


        private IEnumerator RespawnBreakables()
        {
            Breakable[] potsInScene = new Breakable[0];
            WaitForSeconds waitCycle = new WaitForSeconds(0.1f);
            while (potsInScene.Length < breakablesToSpawn)
            {
                yield return waitCycle;
                yield return StartCoroutine(SpawnPotAtRandomLocation());
                potsInScene = FindObjectsOfType<Breakable>();
            }
        }


        private IEnumerator SpawnPotAtRandomLocation()
        {
            bool locationFound = false;
            Vector2 randomCirclePos = Random.insideUnitCircle * spawnRadius;
            Vector3 randomPosition = new Vector3(randomCirclePos.x, hightOffset, randomCirclePos.y);

            while (locationFound == false && checkSpawnForOverlap == true)
            {
                yield return new WaitForSeconds(0.06f);

                randomCirclePos = Random.insideUnitCircle * spawnRadius;
                randomPosition = new Vector3(randomCirclePos.x, hightOffset, randomCirclePos.y);
               
                Collider[] colliders = Physics.OverlapSphere(randomPosition, assetRadius);
                if (colliders.Length == 1)
                {
                    locationFound = true;
                }
            }
            SpawnBreakable(randomPosition);

        }


        private void SpawnBreakable(Vector3 pos)
        {
            GameObject newBreakable = Instantiate(breakblePrefabs[Random.Range(0, breakblePrefabs.Length)], this.transform);
            newBreakable.transform.position = pos;
            newBreakable.transform.rotation = Quaternion.Euler(new Vector3(0, Random.Range(-180f, 180f), 0));
            
            if (randomizeMaterial) newBreakable.GetComponent<Breakable>().RandomizeMaterial(randomHueRange, randomSaturationRange, randomLightnessRange);

            if(spawnEffect) InstantiateEffect(newBreakable);
        }


        private void InstantiateEffect(GameObject spawnedBreakable)
        {
            if (spawnedBreakable == null) return;
            Collider col = spawnedBreakable.GetComponent<MeshCollider>();
            if (col == null) col = spawnedBreakable.GetComponent<BoxCollider>();
            if (col == null) return;
            if (spawnEffectPrefab == null) return;
            GameObject spawnEffect = Instantiate(spawnEffectPrefab, this.transform);
            spawnEffect.transform.position = spawnedBreakable.transform.position;
            spawnEffect.transform.rotation = spawnedBreakable.transform.rotation;
            VisualEffect effect = spawnEffect.GetComponent<VisualEffect>();
            effect.SetVector3("Center", col.bounds.center);
            effect.SetVector3("Size", col.bounds.size);

        }

        
    }
}

