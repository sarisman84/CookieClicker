using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace
{
    public abstract class Modifier : MonoBehaviour
    {
        public int baseScoreCost = 0;
        public float scoreCostMultiplier = 1;
        public ParticleSystem tetherPrefab;
        protected bool HasAlreadyBeenTethered;
        protected int totalScoreCost = 0;

        public UnityEvent onModifierTrigger;

        protected void CalculateTotalScoreCost()
        {
            totalScoreCost = Mathf.RoundToInt(baseScoreCost * scoreCostMultiplier);
          
        }

        public virtual void TriggerModification(GameObject owner)
        {
            if (!HasAlreadyBeenTethered)
            {
                ParticleSystem clone = ObjectPooler.Instantiate(tetherPrefab);
                clone.gameObject.SetActive(true);
                Transform child = clone.transform.GetChild(0);

                Vector3 dirToOwner = transform.position - owner.transform.position;
                Vector3 midPoint = dirToOwner / 2f;

                var localScale = child.localScale;
                Vector3 size = new Vector3(localScale.x, localScale.y, dirToOwner.magnitude - transform.localScale.x - 0.5f);
                Quaternion lookRotation = Quaternion.LookRotation(dirToOwner.normalized);

                clone.transform.position = midPoint;
                clone.transform.rotation = lookRotation;

                ParticleSystem.ShapeModule shape = clone.shape;
                shape.scale = size;
                HasAlreadyBeenTethered = true;
                localScale = size;
                child.localScale = localScale;
            }
            scoreCostMultiplier += 1.5f;
            onModifierTrigger?.Invoke();
        }
    }
}