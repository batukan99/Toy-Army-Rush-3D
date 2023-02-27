
using UnityEngine;
using Game.Core.Data;
using Game.Managers;

namespace Game.Core.Army 
{
    public class RocketArmy : AllyAI
    {
        private SoundManager _soundManager => ManagerProvider.GetManager<SoundManager>();
        public override AllyData GetAllyData() => _AllyData;
        public override void SetAllyData() => _AllyData = Resources.Load<SO_Ally>("Data/SO_Ally_Rocket").AllyData;
        public override void PlayFireSound() => _soundManager.PlayRocketFireSound(AudioSource);

    }

}
