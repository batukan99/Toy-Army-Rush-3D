using UnityEngine;

namespace Game.Core
{
    public abstract class IDamageable: MonoBehaviour
    {
        public abstract void TakeDamage(float damage);

        public abstract Transform GetTransform();

        public abstract bool IsAttackable();
        public abstract bool IsDeath();
        
    }
}