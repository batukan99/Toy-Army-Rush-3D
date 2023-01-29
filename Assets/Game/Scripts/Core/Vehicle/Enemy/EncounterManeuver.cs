using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Game.Core.Vehicle.Enemy 
{
    public class EncounterManeuver : MonoBehaviour, IManeuverBehaviour 
    {
        public void Maneuver(EnemyHelicopter enemyHelicopter) 
        {
            print("encounter");

            enemyHelicopter.transform.DOKill();
            enemyHelicopter.transform.DOLocalMoveY(enemyHelicopter.transform.localPosition.y + enemyHelicopter.MaxHeight, 2f)
                .SetEase(Ease.InOutSine);
            enemyHelicopter.transform.DORotate(new Vector3(0, 25, 0), 2f).SetEase(Ease.InOutSine)
                .OnComplete( () => enemyHelicopter.DecideStrategy() );
        }
    }
}

