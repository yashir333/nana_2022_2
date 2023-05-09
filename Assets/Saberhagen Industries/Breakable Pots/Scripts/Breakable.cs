using UnityEngine;
using UnityEngine.VFX;
using System.Collections;


namespace SaberhagenIndustries
{
    public class Breakable : MonoBehaviour
    {
        [Header("Setup")]
        [SerializeField] private GameObject brokenPrefab;

        [Header("Dissolve")]
        [SerializeField] private float shrinkDelay = 6F;
        [SerializeField] private float shrinkDuration = 3F;
        
        [Header("Damage Interaction")]
        [SerializeField] private float health = 200f;

        private bool materialWasRandomized = false;
        private GameObject brokenGO;
        private Material material;


        public void RandomizeMaterial(Vector2 randomHueRange, Vector2 randomSaturationRange, Vector2 randomLightnessRange)
        {
            material = GetComponent<Renderer>().material;
            material.SetFloat("_Hue", Random.Range(randomHueRange.x, randomHueRange.y));
            material.SetFloat("_Saturation", Random.Range(randomSaturationRange.x, randomSaturationRange.y));
            material.SetFloat("_Lightness", Random.Range(randomLightnessRange.x, randomLightnessRange.y));
            materialWasRandomized = true;
        }


        public void TriggerBreak()
        {
            DestoryIntactObject();
            InstantiateBrokenPrefab();
            PropagateMaterialToBrokenPieces();


            StartCoroutine(Failsafe());
            ApplyShrinkDissolveToPieces();
        }


        public void TakeDamage(float dmg)
        {
            if (health <= 0f) return;
            health -= dmg;
            if (health > 0) return;
            health = 0f;
            TriggerBreak();
        }


        private void DestoryIntactObject()
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            Renderer renderer = GetComponent<Renderer>();
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            MeshCollider collider = GetComponent<MeshCollider>();
            BoxCollider boxCollider = GetComponent<BoxCollider>();
            Destroy(boxCollider);
            Destroy(collider);
            Destroy(rb);
            Destroy(renderer);
            Destroy(meshFilter);
        }


        private void InstantiateBrokenPrefab()
        {
            if (brokenPrefab == null) return;
            brokenGO = Instantiate(brokenPrefab, this.transform);
        }
        

        private void PropagateMaterialToBrokenPieces()
        {
            if (materialWasRandomized == false) return;
            Renderer[] renderers = brokenGO.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                renderer.material = material;
            }
        }


        private IEnumerator Failsafe()
        {
            yield return new WaitForSeconds(shrinkDelay * 8f);
            Destroy(this.gameObject);
        }


        private void ApplyShrinkDissolveToPieces()
        {
            Rigidbody[] rbs = brokenGO.GetComponentsInChildren<Rigidbody>();

            foreach (Rigidbody rb in rbs)
            {
                StartCoroutine(TriggerShrinkWhenSleeping(rb));
            }
        }


        private IEnumerator TriggerShrinkWhenSleeping(Rigidbody rb)
        {
            float shrinkDelayRng = Random.Range(shrinkDelay - (shrinkDelay * 0.2F), shrinkDelay + (shrinkDelay * 0.2F));
            float failsafeTime = Time.time + shrinkDelayRng * 4f;
            WaitForSeconds checkCycle = new WaitForSeconds(0.1F);
            while (rb.IsSleeping() == false && Time.time < failsafeTime)
            {
                yield return checkCycle;
            }
            
            yield return new WaitForSeconds(shrinkDelayRng);

            StartCoroutine(ShrinkAndKill(rb.transform));

        }


        private IEnumerator ShrinkAndKill(Transform tf)
        {
            Vector3 currentScale = tf.localScale;
            float shrinktimeRng = Random.Range(shrinkDuration - (shrinkDuration * 0.2F), shrinkDuration + (shrinkDuration * 0.2F));
            float t = 0f;
            while (t < 1)
            {
                t += Time.deltaTime / shrinktimeRng;
                tf.localScale = Vector3.Lerp(currentScale, Vector3.zero, t);
                yield return null;
            }
            Destroy(tf.gameObject);
        }

    }
}


