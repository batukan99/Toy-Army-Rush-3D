using System;
using UnityEngine;
using Game.Core.Enums;
namespace Game.Core.Data
{
    [Serializable]
    public class PlayerData
    {
        public Vector2 ClampPosition = new Vector2(-5, 5);
        public int MaxSideBlocks;
        public int MaxForwardBlock;
        public float SidewaySpeed;
        public float ForwardSpeed;
    }
}