using System;
using Game.Core.Enums;
namespace Game.Core.Data
{
    [Serializable]
    public class EnemyData
    {
        public EnemyType enemyType;
        public int Health;
        public int Damage;
        public float AttackDelay;
        public float AttackRange;
        public float WaitTime;
    }
}