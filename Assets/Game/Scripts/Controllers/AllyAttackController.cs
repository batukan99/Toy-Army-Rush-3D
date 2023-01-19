using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Managers;
using Game.Core.Army;
using Game.Core;
using Game.Core.Enums;

namespace Game.Controllers 
{
    public class AllyAttackController : MonoBehaviour
    {
        [SerializeField]
        public Transform MuzzleTransform;
        [SerializeField]
        private AllyAI allyAI;

        public IDamageable EnemyTarget;
        private float _attackDelay => allyAI.GetAllyData().AttackDelay;

        private const int ALLY_BULLET_LAYER = 6;

        private PoolManager _poolManager;
        private void Awake() {
            _poolManager = ManagerProvider.GetManager<PoolManager>();
        }

        
        public Coroutine StartAttackCoroutine() 
        {
            return allyAI.attackState.AttackCoroutine = StartCoroutine(Attack());
        }
        private IEnumerator Attack()
        {
            WaitForSeconds Wait = new WaitForSeconds(_attackDelay);
            

            yield return Wait;

            if (EnemyTarget != null && !EnemyTarget.IsDeath() && !allyAI.IsDeath())
            {
                allyAI.Animator.SetTrigger("Shoot");

                Vector3 muzzleTarget = new Vector3(EnemyTarget.transform.position.x, 
                        EnemyTarget.transform.position.y + Random.Range(1, 2), EnemyTarget.transform.position.z);
                MuzzleTransform.LookAt(muzzleTarget);

                Bullet bullet = null;
                if(allyAI._AllyData.allyType == AllyType.Rifle || allyAI._AllyData.allyType == AllyType.Sniper)
                {
                    bullet = _poolManager.GetBulletObject(MuzzleTransform.position, MuzzleTransform.rotation, null);
                } 
                else if (allyAI._AllyData.allyType == AllyType.Rocket) 
                {
                    bullet = _poolManager.GetHomingBulletObject(MuzzleTransform.position, MuzzleTransform.rotation, null);
                }
                if (bullet != null)
                {
                    bullet.SetLayer(ALLY_BULLET_LAYER);
                    bullet.Shoot(MuzzleTransform.rotation, EnemyTarget.transform);
                }
            }
            
            EnemyTarget = null;
            yield return Wait;
            
            allyAI.enemies.RemoveAll(allyAI.DisabledDamageables);


            allyAI.attackState.AttackCoroutine = null;
        }
    }

}

