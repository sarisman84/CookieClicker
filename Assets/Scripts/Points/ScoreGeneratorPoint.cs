using UnityEngine;

namespace DefaultNamespace
{
    public class ScoreGeneratorPoint : BasePoint
    {
        protected override void OnModificationBought()
        {
            CookieManager.SingletonAccess.PointGenerationRate =
                Mathf.Abs(CookieManager.SingletonAccess.PointGenerationRate / 2f);
            CookieManager.SingletonAccess.PointGenerationAmm += CookieManager.SingletonAccess.PointGenerationAmm + 1;
        }
    }
}