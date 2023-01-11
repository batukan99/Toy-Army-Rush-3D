using UnityEngine;
using DG.Tweening;
using Game.Managers;
using Game.UI;

namespace Game.Core.Obstacles
{
    public class DestructibleBase : IDamageable
    {
        [SerializeField] protected int damage = 5;

        [SerializeField] protected HealthUI healthUI;
        public float health;
        protected Collider _collider;
        private UIManager _uiManager;
        protected bool _isDeath;
        protected virtual void Awake() 
        {
            //base.Awake();
            _uiManager = ManagerProvider.GetManager<UIManager>();
            healthUI.transform.SetParent(_uiManager.WorldSpaceCanvas.transform);
        }

        public void Disappear()
        {
            gameObject.SetActive(false);
            healthUI.gameObject.SetActive(false);
            _isDeath = true;
        }

        public virtual void BreakApart()
        {
            _isDeath = true;
            Disappear();

            //MMVibrationManager.Haptic(HapticTypes.LightImpact);
        }
        public override void TakeDamage(float damage)
        {
            health -= damage;
            
            if (health <= 0)
            {
                health = 0;
                BreakApart();
            }
            healthUI.SetHealth(health);
            
            transform.DOKill(true);
            transform.DOScale(transform.localScale * 1.12f, 0.08f).SetLoops(2, LoopType.Yoyo);
        }

        public override Transform GetTransform() => transform;

        public override bool IsAttackable() => !IsDeath();

        public override bool IsDeath() => _isDeath;
    }
}
