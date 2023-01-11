using UnityEngine;

namespace Game.Core.Obstacles
{
    public class ObstacleBase
    {
        [SerializeField] protected int damage = 5;

        protected Collider _collider;

        protected virtual void Awake()
        {
            //_collider = GetComponent<Collider>();
        }
    }
}
