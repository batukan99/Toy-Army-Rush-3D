using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.Vehicle
{
    public class Piece : MonoBehaviour
    {
        public Collider Collider { get; private set; }

        [Range(1, 10)]
        public int Capacity = 1;
        public float Length;

        //[Range(0, 100)]
        public float Health;
        public Transform ArmyPlace;
        

        [HideInInspector]
        public int numberOfArmyAdded;
        

        private void Awake()
        {
            Collider = GetComponent<Collider>();
        }
    }

}
