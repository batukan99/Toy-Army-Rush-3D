
using System.Collections.Generic;
using UnityEngine;
using Game.Core.Army;
using Game.Core;

namespace Game.StateMachines.Enemy
{
    public class AimState : State
    {
        public EnemyAI enemyAI;
        private IDamageable _target;
        public AimState(EnemyAI enemyAI, ArmyStateMachine stateMachine) 
        : base(stateMachine)
        {
            this.enemyAI = enemyAI;
        }

        public override void Enter()
        {
            base.Enter();
            enemyAI.Animator.SetTrigger("Aim");
        }

        public override void Exit()
        {
            base.Exit();
            enemyAI.Animator.ResetTrigger("Aim");
        }

        public override void HandleInput()
        {
            base.HandleInput();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if(enemyAI.CurrentTarget != null)
            {
                enemyAI.allies.RemoveAll(enemyAI.DisabledDamageables);
                if(enemyAI.CurrentTarget.IsDeath()) 
                {
                    Debug.Log("there was a death on list");
                    enemyAI.CurrentTarget = null;
                }     
            }

            _target = enemyAI.GetClosestDamageable();
            
            if(_target != null) 
            {
                enemyAI.CurrentTarget = _target;
                enemyAI.AimedAtTarget = true;
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
    }

}