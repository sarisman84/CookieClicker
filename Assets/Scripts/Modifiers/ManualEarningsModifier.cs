using UnityEngine;

namespace DefaultNamespace
{
    public class ManualEarningsModifier : Modifier
    {
        public int increaseManualEarningsAmmBy = 1;


        public override void TriggerModification(GameObject owner)
        {
            CalculateTotalScoreCost();
            if (totalScoreCost <= CookieManager.SingletonAccess.CurrentScore)
            {
                base.TriggerModification(owner);
                CookieManager.SingletonAccess.CurrentClickIncrementAmm +=
                    Mathf.RoundToInt(increaseManualEarningsAmmBy * scoreCostMultiplier / 4f);
                CookieManager.SingletonAccess.CurrentScore -= totalScoreCost;
                CookieManager.SingletonAccess.UpdateScoreDisplay();
                CookieManager.SingletonAccess.UpdateIncrementAmmDisplay();
            }
        }
    }
}