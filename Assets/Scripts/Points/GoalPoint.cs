using UnityEngine;

namespace DefaultNamespace
{
    public class GoalPoint : BasePoint
    {
        protected override void OnModificationBought()
        {
            Debug.Log("You Won!");
        }
    }
}