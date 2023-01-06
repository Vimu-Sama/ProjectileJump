using UnityEngine;

namespace Level
{
    public class PlatformCheckScript : MonoBehaviour
    {
        private AutomatedLevelGeneration levelGeneratorInstance= null;
        private void Start()
        {
            if(!levelGeneratorInstance)
                levelGeneratorInstance = AutomatedLevelGeneration.Instance;
        }

        private void OnTriggerExit(Collider other)
        {
            levelGeneratorInstance.ChangePlatformPosition(gameObject);
        }
    }
}
