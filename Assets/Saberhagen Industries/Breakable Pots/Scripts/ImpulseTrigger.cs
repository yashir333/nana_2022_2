using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace SaberhagenIndustries
{
    public class ImpulseTrigger : MonoBehaviour
    {
        [SerializeField] private float impulseDelay = 1F;
        [SerializeField] private bool randomizeDelay = false;

        [SerializeField]
        private UnityEvent OnTrigger = new UnityEvent();

        private float delay = 1f;  

        void Start()
        {
            delay = impulseDelay;
            if (randomizeDelay) delay = Random.Range(impulseDelay * 0.8f, impulseDelay * 1.2f);
            StartCoroutine(TriggerAutoImpulse());
        }


        private IEnumerator TriggerAutoImpulse()
        {
            yield return new WaitForSeconds(delay);
            OnTrigger.Invoke();
        }


    }
}


