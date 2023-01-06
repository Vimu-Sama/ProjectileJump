using UnityEngine;
using System.Collections.Generic;

namespace Level
{
    public class AutomatedLevelGeneration : GenericSingleton<AutomatedLevelGeneration>
    {
        [SerializeField] private float xMinima;
        [SerializeField] private float xMaxima;
        [SerializeField] private float yMinima;
        [SerializeField] private float yMaxima;
        [SerializeField] Vector3 lastPlatformPosition;
        private Vector3 tempTransformVector;

        public void ChangePlatformPosition(GameObject platformGameObject)
        {
            tempTransformVector.x = Random.Range(xMinima, xMaxima) + lastPlatformPosition.x;
            tempTransformVector.y = Random.Range(yMinima, yMaxima) + lastPlatformPosition.y ;
            platformGameObject.transform.position = tempTransformVector;
            lastPlatformPosition = tempTransformVector;
        }
    }
}
