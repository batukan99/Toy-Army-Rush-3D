
using System.Collections.Generic;
using UnityEngine;
using Game.Core.Army;

namespace Game.StateMachines.Enemy
{
    public class DeathState : State
    {
        public EnemyAI enemyAI;
        public DeathState(EnemyAI enemyAI, ArmyStateMachine stateMachine) 
        : base(stateMachine)
        {
            this.enemyAI = enemyAI;
        }

        public override void Enter()
        {
            base.Enter();
            enemyAI.Animator.SetTrigger("Die");
            Debug.Log("enemy army died");
        }

        public override void Exit()
        {
            base.Exit();
            enemyAI.Animator.ResetTrigger("Die");
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