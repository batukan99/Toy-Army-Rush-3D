using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Managers;
using Game.Core.Army;
using Game.Core;

namespace Game.Controllers 
{
    public class EnemyAttackController : MonoBehaviour
    {
        [SerializeField]
        public Transform MuzzleTransform;
        [SerializeField]
        private EnemyAI enemyAI;

        public IDamageable AllyTarget;
        private float _attackDelay => enemyAI.GetEnemyData().AttackDelay;

        private PoolManager _poolManager;
        private void Awake() {
            _poolManager = ManagerProvider.GetManager<PoolManager>();
        }
        
        public Coroutine StartAttackCoroutine() 
        {
            return enemyAI.attackState.AttackCoroutine = StartCoroutine(Attack());
        }
        private IEnumerator Attack()
        {
            WaitForSeconds Wait = new WaitForSeconds(_attackDelay);

            yield return Wait;
            
            if (AllyTarget != null && !AllyTarget.IsDeath())
            {
                Vector3 muzzleTarget = new Vector3(AllyTarget.transform.position.x, 
                        AllyTarget.transform.position.y + Random.Range(1, 2), AllyTarget.transform.position.z);
                MuzzleTransform.LookAt(muzzleTarget);
                Bullet bullet = _poolManager.GetEnemyBulletObject(MuzzleTransform.position, MuzzleTransform.rotation, null);
                if (bullet != null)
                {
                    bullet.Shoot(MuzzleTransform.rotation);
                }
            }

            AllyTarget = null;

            yield return Wait;

            enemyAI.allies.RemoveAll(enemyAI.DisabledDamageables);


            enemyAI.attackState.AttackCoroutine = null;
        }
    }

}

