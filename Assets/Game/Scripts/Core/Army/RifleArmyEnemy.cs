using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core.Data;

namespace Game.Core.Army 
{
    public class RifleArmyEnemy : EnemyAI
    {
        public override EnemyData GetEnemyData() => _EnemyData;
        public override void SetEnemyData() => _EnemyData = Resources.Load<SO_Enemy>("Data/SO_Enemy_Rifle").EnemyData;

    }

}
