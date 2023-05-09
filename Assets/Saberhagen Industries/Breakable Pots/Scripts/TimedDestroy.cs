using System.Collections;
using UnityEngine;


namespace SaberhagenIndustries
{
    public class TimedDestroy : MonoBehaviour
    {

        [SerializeField] private float destroyDelay = 1f;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(destroyDelay);
            Destroy(this.gameObject);
        }

        
    }
}

