using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core.Army;
using Game.Core;
using Game.Controllers;
using DG.Tweening;

namespace Game.StateMachines.Enemy 
{
    public class AttackState : State
    {
        public EnemyAI enemyAI;
        private IDamageable _target;
        public Coroutine AttackCoroutine;
        private EnemyAttackController _attackController;
        public AttackState(EnemyAI enemyAI, ArmyStateMachine stateMachine, EnemyAttackController attackController) 
        : base(stateMachine)
        {
            this.enemyAI = enemyAI;
            this._attackController = attackController;
        }

        public override void Enter()
        {
            base.Enter();
            enemyAI.Animator.SetTrigger("Shoot");
            _target = enemyAI.CurrentTarget;
        }

        public override void Exit()
        {
            base.Exit();
            enemyAI.Animator.ResetTrigger("Shoot");
        }

        public override void HandleInput()
        {
            base.HandleInput();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if(_target)
            {
                Transform targetTransform = _target.GetTransform();
                Vector3 targetPosition = new Vector3(targetTransform.position.x, enemyAI.GetTransform().position.y, targetTransform.position.z);
                enemyAI.GetTransform().DOLookAt(targetPosition, 0.05f);
                if (AttackCoroutine == null)
                {
                    _attackController.AllyTarget = _target;
                    _attackController.StartAttackCoroutine();
                }
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            if(_target)
            {
                if(Vector3.Distance(enemyAI.GetTransform().position, _target.GetTransform().position) >= enemyAI._EnemyData.AttackRange)
                 //if(_target.GetTransform().position.z > enemyAI.GetTransform().position.z)
                {
                    _target = null;
                    enemyAI.AimedAtTarget = false;
                    enemyAI.SetTargetAllyArmy(null);
                }
            }
        }
    }

}