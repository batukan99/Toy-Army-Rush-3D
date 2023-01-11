
using UnityEngine;

namespace Game.Core.Data
{
    [System.Serializable]
    public class PiecePlacementData
    {
        [HideInInspector] public bool isUsed = false;
        public Transform position;
        public int currentArmyCount;


        //public PoseType pose;
        //public FellowColorType color;
    }
}
