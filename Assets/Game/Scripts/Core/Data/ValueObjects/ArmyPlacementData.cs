
using UnityEngine;

namespace Game.Core.Data
{
    [System.Serializable]
    public class ArmyPlacementData
    {
        [HideInInspector] public bool isUsed = false;
        public Transform position;
        //public PoseType pose;
        //public FellowColorType color;
    }
}
