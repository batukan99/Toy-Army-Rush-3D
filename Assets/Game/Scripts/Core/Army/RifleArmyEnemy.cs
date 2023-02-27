using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core.Data;
using Game.Managers;

namespace Game.Core.Army 
{
    public class RifleArmyEnemy : EnemyAI
    {
        private SoundManager _soundManager => ManagerProvider.GetManager<SoundManager>();
        public override EnemyData GetEnemyData() => _EnemyData;
        public override void SetEnemyData() => _EnemyData = Resources.Load<SO_Enemy>("Data/SO_Enemy_Rifle").EnemyData;
        public override void PlayFireSound() => _soundManager.PlayRifleFireSound(AudioSource);

    }

}
