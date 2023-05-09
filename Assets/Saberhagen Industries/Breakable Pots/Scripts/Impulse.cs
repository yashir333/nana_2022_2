using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SaberhagenIndustries
{
    public class Impulse : MonoBehaviour
    {
        [SerializeField] private bool applyDamage = true;
        [SerializeField] private bool applyForce = true;

        [SerializeField] private float impulseRadius = 5F;
        [SerializeField] private float impulseForce = 50F;
        [SerializeField] private float impulseDamage = 420F;

        [SerializeField] private AnimationCurve falloffForce = AnimationCurve.Linear(0f,1f,1f,0f);
        [SerializeField] private AnimationCurve falloffDamage = AnimationCurve.Linear(0f, 1f, 1f, 0f);

        private float randomizedImpulseRadius;
        private float randomizedImpulseForce;
        private float randomizedImpulseDamage;

        private Collider[] hitColliders;

        public void GenerateImpulse()
        {
            RandomizeImpulseValues();

            if (applyDamage) StartCoroutine(GenerateImpulseDamage());
            if (applyForce) StartCoroutine(GenerateImpulseForce());

            StartCoroutine(GenerateImpulseForce());
        }
        


        private void RandomizeImpulseValues()
        {
            float randomFraction = Random.Range(0.9f, 1.1f);
            randomizedImpulseRadius = impulseRadius * randomFraction;
            randomizedImpulseForce = impulseForce * randomFraction;
            randomizedImpulseDamage = impulseDamage * randomFraction;
        }


        private void FindHitColliders()
        {
            hitColliders = Physics.OverlapSphere(this.transform.position, randomizedImpulseRadius);
        }


        private IEnumerator GenerateImpulseDamage()
        {
            FindHitColliders();

            foreach (Collider foundCollider in hitColliders)
            {
                if (foundCollider == null) continue;
                Breakable breakable;
                if (foundCollider.TryGetComponent<Breakable>(out breakable))
                {
                    Vector3 colliderPos = foundCollider.transform.position;
                    float distance = Vector3.Distance(colliderPos, this.transform.position);
                    float distanceFraction = (randomizedImpulseRadius - distance) / randomizedImpulseRadius;

                    breakable.TakeDamage(randomizedImpulseDamage * falloffDamage.Evaluate(1f - distanceFraction));
                }
            }
            yield break;
        }


        private IEnumerator GenerateImpulseForce()
        {
            yield return new WaitForFixedUpdate();

            FindHitColliders();

            foreach (Collider foundCollider in hitColliders)
            {
                if (foundCollider == null) continue;
                Rigidbody rb;
                if (foundCollider.TryGetComponent<Rigidbody>(out rb))
                {
                    Vector3 colliderPos = foundCollider.transform.position;
                    float distance = Vector3.Distance(colliderPos, this.transform.position);
                    float distanceFraction = (randomizedImpulseRadius - distance) / randomizedImpulseRadius;

                    rb.AddExplosionForce(randomizedImpulseForce * falloffForce.Evaluate(1f - distanceFraction), this.transform.position, randomizedImpulseRadius, 0.0F, ForceMode.Impulse);
                }
            }
        }

        
    }
}

