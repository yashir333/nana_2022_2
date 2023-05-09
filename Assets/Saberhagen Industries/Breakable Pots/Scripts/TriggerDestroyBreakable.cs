using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaberhagenIndustries
{
    public class TriggerDestroyBreakable : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            Breakable breakable;
            if (other.TryGetComponent<Breakable>(out breakable))
            {
                Destroy(breakable.gameObject);
            }
        }
    }
}

