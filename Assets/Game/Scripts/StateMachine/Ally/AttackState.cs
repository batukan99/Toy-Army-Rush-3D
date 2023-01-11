using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Game.Core.Army;
using Game.Controllers;
using DG.Tweening;


namespace Game.StateMachines.Ally 
{
    public class AttackState : State
    {
        public AllyAI allyAI;
        private IDamageable _target;
        public Coroutine AttackCoroutine;
        private AllyAttackController _attackController;
        
        public AttackState(AllyAI allyAI, ArmyStateMachine stateMachine, AllyAttackController attackController) 
        : base(stateMachine)
        {
            this.allyAI = allyAI;
            this._attackController = attackController;
        }

        public override void Enter()
        {
            base.Enter();
            _target = allyAI.CurrentTarget;
            Debug.Log("attackEntered");
        }

        public override void Exit()
        {
            base.Exit();
            allyAI.Animator.ResetTrigger("Shoot");
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
                Vector3 targetPosition = new Vector3(targetTransform.position.x, allyAI.GetTransform().position.y, targetTransform.position.z);
                allyAI.GetTransform().DOLookAt(targetPosition, 0.05f);
                if (AttackCoroutine == null)
                {
                    _attackController.EnemyTarget = _target;
                    _attackController.StartAttackCoroutine();
                }
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            if(_target)
            {
                if(Vector3.Distance(allyAI.GetTransform().position, _target.GetTransform().position) >= allyAI._AllyData.AttackRange)
                //if(Camera.main.transform.position.z > _target.GetTransform().position.z) 
                {
                    _target = null;
                    allyAI.AimedAtTarget = false;
                    allyAI.SetTargetEnemyArmy(null);
                }
            }
        }

    }

}