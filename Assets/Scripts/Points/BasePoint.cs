using System;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public abstract class BasePoint : MonoBehaviour
    {
        public Material modAvailableTetherMat;
        public Material modBoughtTetherMat;
        public Material modUnavailableTetherMat;
        public ParticleSystem tetherPrefab;
        [Space] public int modBaseCost = 0;
        public int modBuyAmm = 1;
        public BasePoint requiredBasePoint;
        [Space] public UnityEvent onModPurchase;


        private int _curBuyAmm = 0;

        protected ParticleSystemRenderer TetherParticleRenderer;
        protected Renderer TetherRenderer;
        protected ParticleSystem TetherClone;

        [Space] [SerializeField] private int totalCost;
        [SerializeField] private float distFromCore;


        public int TotalCost
        {
            get
            {
                int totalCost = Mathf.RoundToInt(modBaseCost * Mathf.CeilToInt(Vector3.Distance(transform.position,
                    CookieManager.SingletonAccess.transform.position)));
                return totalCost;
            }
        }


        private void OnEnable()
        {
            _curBuyAmm = modBuyAmm;
        }

        public void TriggerModification(GameObject owner)
        {
            if (_curBuyAmm > 0 && CanModBeBought())
            {
                onModPurchase?.Invoke();
                OnModificationBought();
                CookieManager.SingletonAccess.CurrentScore -= TotalCost;
                if (_curBuyAmm > 1)
                    modBaseCost = Mathf.RoundToInt(modBaseCost * 6.75f);
                _curBuyAmm--;
                CookieManager.SingletonAccess.UpdateScoreDisplay();
                CookieManager.SingletonAccess.UpdateIncrementAmmDisplay();
                CookieManager.SingletonAccess.AddModifierToRegistry(this);
            }
        }

        protected abstract void OnModificationBought();


        private void Update()
        {
            ShowDisplayTether(requiredBasePoint
                ? requiredBasePoint.transform
                : CookieManager.SingletonAccess.transform);

            totalCost = TotalCost;
            distFromCore = Vector3.Distance(transform.position,
                CookieManager.SingletonAccess.transform.position);
        }


        private void ShowDisplayTether(Transform objToTether)
        {
            if (!TetherClone)
                SpawnTether(objToTether);


            bool canBeBought = CanModBeBought();

            Material mat = _curBuyAmm == 0 ? modBoughtTetherMat :
                canBeBought ? modAvailableTetherMat : modUnavailableTetherMat;

            TetherParticleRenderer = TetherParticleRenderer
                ? TetherParticleRenderer
                : TetherClone.GetComponent<ParticleSystemRenderer>();
            TetherRenderer = TetherRenderer
                ? TetherRenderer
                : TetherClone.transform.GetChild(0).GetComponent<MeshRenderer>();

            TetherParticleRenderer.material = mat;
            TetherRenderer.material = mat;
        }

        private bool CanModBeBought()
        {
            bool canSelfBeBought = TotalCost <= CookieManager.SingletonAccess.CurrentScore;
            if (requiredBasePoint)
                return canSelfBeBought && requiredBasePoint.CanModBeBought() &&
                       CookieManager.SingletonAccess.HasModification(requiredBasePoint);
            return canSelfBeBought;
        }


        private void SpawnTether(Transform objToTether)
        {
            if (!TetherClone)
            {
                objToTether = objToTether ? objToTether : requiredBasePoint.transform;

                ParticleSystem clone = ObjectPooler.Instantiate(tetherPrefab);
                clone.gameObject.SetActive(true);

                AlignTetherToTarget(clone, objToTether);

                TetherClone = clone;
            }
        }

        private void AlignTetherToTarget(ParticleSystem system, Transform target)
        {
            Transform child = system.transform.GetChild(0);

            Vector3 dirToOwner = transform.position - target.position;

            Vector3 midPoint = dirToOwner / 2f + target.position;

            var localScale = child.localScale;
            Vector3 size = new Vector3(localScale.x, localScale.y,
                dirToOwner.magnitude - transform.localScale.x - 0.5f);
            Quaternion lookRotation = Quaternion.LookRotation(dirToOwner.normalized);

            system.transform.position = midPoint;
            system.transform.rotation = lookRotation;

            ParticleSystem.ShapeModule shape = system.shape;
            shape.scale = size;
            localScale = size;
            child.localScale = localScale;
        }


#if UNITY_EDITOR
        private Color _debugColor;

        protected void OnValidate()
        {
            _debugColor = new Color(Random.Range(0, 2), Random.Range(0, 2), Random.Range(0, 2), 1);
        }


        protected void OnDrawGizmos()
        {
            if (requiredBasePoint)
            {
                Gizmos.color = _debugColor;
                Gizmos.DrawLine(transform.position, requiredBasePoint.transform.position);
            }
        }
#endif
    }
}