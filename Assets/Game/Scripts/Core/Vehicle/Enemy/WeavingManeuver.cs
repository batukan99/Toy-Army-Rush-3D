
using UnityEngine;
using DG.Tweening;

namespace Game.Core.Vehicle.Enemy 
{
    public class WeavingManeuver : MonoBehaviour, IManeuverBehaviour 
    {
        private EnemyHelicopter _enemyHelicopter;
        public void Maneuver(EnemyHelicopter enemyHelicopter) 
        {
            print("waeve");
            this._enemyHelicopter = enemyHelicopter;
            _enemyHelicopter.transform.DOKill();

            _enemyHelicopter.transform.DORotate(new Vector3(0, 25, 0), 1f).OnComplete( () => Weave() );
        }

        private void Weave()
        {
            Sequence seq = DOTween.Sequence();

            _enemyHelicopter.MoveEnemyToOtherSide();

            seq.Append(_enemyHelicopter.transform.DOMoveX(_enemyHelicopter.WeavingDistance, 3f).SetSpeedBased(true)
                .SetEase(Ease.InOutSine));
            seq.Append(_enemyHelicopter.transform.DORotate(new Vector3(0, -25, 0), 1.5f)
                .SetEase(Ease.Linear));
            
            seq.OnComplete( () =>  ChangeStrategy() );
        }
        
        private void ChangeStrategy() 
        {
            _enemyHelicopter.MoveEnemyToOtherSide();
            _enemyHelicopter.ApplyStrategy( _enemyHelicopter.GetComponent<BopplingManeuver>() );
        }

    }
}
