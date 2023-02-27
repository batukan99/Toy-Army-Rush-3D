
using UnityEngine;
using Game.Core.Data;
using Game.Managers;

namespace Game.Core.Army 
{
    public class RifleArmy : AllyAI
    {
        private SoundManager _soundManager => ManagerProvider.GetManager<SoundManager>();
        public override AllyData GetAllyData() => _AllyData;
        public override void SetAllyData() => _AllyData = Resources.Load<SO_Ally>("Data/SO_Ally_Rifle").AllyData;
        public override void PlayFireSound() => _soundManager.PlayRifleFireSound(AudioSource);

    }

}
