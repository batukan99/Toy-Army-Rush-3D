using System;
using Game.Core.Enums;

namespace Game.Core.Data
{
    [Serializable]
    public class BulletData
    {
        public AllyType allyType;
        public float AutoDestroyTime;
        public float SpawnDelay;
        public float MoveSpeed;
        public float Damage;
    }
}