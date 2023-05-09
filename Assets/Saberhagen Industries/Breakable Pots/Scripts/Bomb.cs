using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;


namespace SaberhagenIndustries
{
    public class Bomb : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject bomb;
        [SerializeField] private GameObject fuse;
        [SerializeField] private GameObject fuseEffect;
        [SerializeField] private GameObject explosionEffect;

        [Header("Settings")]
        [SerializeField] private bool autoLightFuse = true;
        [SerializeField] private float fuseTimer = 3f;
        [SerializeField] private float fuseDelay = .6f;
        [SerializeField] private float explosionDelay = 1f;

        [SerializeField] private UnityEvent OnExplosion = new UnityEvent();

        private Rigidbody rb;
        private MeshCollider mc;
        private VisualEffect fuseVfx;
        private VisualEffect explosionVfx;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            mc = GetComponent<MeshCollider>();
            fuseVfx = fuseEffect.GetComponent<VisualEffect>();
            explosionVfx = explosionEffect.GetComponent<VisualEffect>();    
            if (autoLightFuse) StartCoroutine(IgniteFuse());
        }

        public void LightFuse()
        {
            StartCoroutine(IgniteFuse());
        }

        private IEnumerator IgniteFuse()
        {
            Vector3 fuseStartPos;
            Material fuseMaterial;

            float randomizedFuseTimer = Random.Range(fuseTimer * 0.8f, fuseTimer * 1.2f);
            float randomizedFuseDelay = Random.Range(fuseDelay * 0.8f, fuseDelay * 1.2f);
            float randomizedExplosionDelay = Random.Range(explosionDelay * 0.8f, explosionDelay * 1.2f);

            explosionVfx.SetFloat("Explosion_Size", Random.Range(0f, 1f));

            fuseMaterial = fuse.GetComponent<Renderer>().material;
            fuseStartPos = fuseEffect.transform.localPosition;

            yield return new WaitForSeconds(randomizedFuseDelay);

            fuseEffect.SetActive(true);

            float t = 0f;
            while (t < 1)
            {
                t += Time.deltaTime / randomizedFuseTimer;
                fuseMaterial.SetFloat("_Fuse", Mathf.Lerp(1f, 0f, t));
                fuseEffect.transform.localPosition = Vector3.Lerp(fuseStartPos, fuse.transform.localPosition - new Vector3(0f,0.1f,0f), t);
                fuseVfx.SetFloat("Spray_Rate", Mathf.Lerp(1f, 0f, t));
                fuseVfx.SetFloat("Spray_Spread", Mathf.Lerp(1f, 0f, t));
                yield return null;
            }

            yield return new WaitForSeconds(randomizedExplosionDelay);
            fuseEffect.SetActive(false);

            bomb.SetActive(false);  
            explosionEffect.transform.rotation = Quaternion.identity;  
            explosionEffect.SetActive(true);
            Destroy(rb);
            Destroy(mc);  
            OnExplosion.Invoke();   

            yield return new WaitForSeconds(2f);
            Destroy(this.gameObject);
        }
    }
}

