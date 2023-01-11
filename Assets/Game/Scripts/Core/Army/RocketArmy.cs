
using UnityEngine;
using Game.Core.Data;

namespace Game.Core.Army 
{
    public class RocketArmy : AllyAI
    {
        public override AllyData GetAllyData() => _AllyData;
        public override void SetAllyData() => _AllyData = Resources.Load<SO_Ally>("Data/SO_Ally_Rocket").AllyData;

    }

}
