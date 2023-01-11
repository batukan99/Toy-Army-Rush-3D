using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core.Data;

namespace Game.Core.Army 
{
    public class PistolArmy : AllyAI
    {
        public override AllyData GetAllyData() => _AllyData;
        public override void SetAllyData() => _AllyData = Resources.Load<SO_Ally>("Data/SO_Ally_Pistol").AllyData;

    }

}