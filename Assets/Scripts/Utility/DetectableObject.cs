using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace
{
    public class DetectableObject : MonoBehaviour
    {
        public UnityEvent<GameObject> onDetectionTrigger;
        public UnityEvent<GameObject> onDetectionHover;
    }
}