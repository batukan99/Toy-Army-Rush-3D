
using UnityEngine;
using DG.Tweening;

namespace Game.Core.Vehicle.Enemy 
{
    public class BopplingManeuver : MonoBehaviour, IManeuverBehaviour 
    {
        public void Maneuver(EnemyHelicopter enemyHelicopter) 
        {
            print("boppling");
            //StartCoroutine(Bopple(enemyHelicopter));
            enemyHelicopter.transform.DOKill();
            enemyHelicopter.transform.DOMoveY(enemyHelicopter.MaxHeight, 1).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        }
    }
}
