using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.Vehicle.Enemy 
{
    public interface IManeuverBehaviour
    {
        void Maneuver(EnemyHelicopter enemyHelicopter);
    }
}