using UnityEngine;

namespace SaberhagenIndustries
{
    public class FramerateLimiter : MonoBehaviour
    {
        [SerializeField] private int targetFPS = 60;

        private void Awake()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = targetFPS;
        }
    }

}
