using System.Collections;
using System.Collections.Generic;
using Game.Core.Army;
using UnityEngine;

namespace Game.StateMachines.Enemy 
{
    public class IdleState : State
    {
        public EnemyAI enemyAI;
        public IdleState(EnemyAI enemyAI, ArmyStateMachine stateMachine) 
        : base(stateMachine)
        {
            this.enemyAI = enemyAI;
        }

        public override void Enter()
        {
            base.Enter();
            enemyAI.Animator.SetTrigger("Idle");
        }

        public override void Exit()
        {
            base.Exit();
            enemyAI.Animator.ResetTrigger("Idle");
        }

        public override void HandleInput()
        {
            base.HandleInput();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
    }

}
