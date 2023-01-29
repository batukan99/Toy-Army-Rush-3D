
using System.Collections.Generic;
using UnityEngine;
using Game.Core.Army;

namespace Game.StateMachines.Ally 
{
    public class DeathState : State
    {
        public AllyAI allyAI;
        public DeathState(AllyAI allyAI, ArmyStateMachine stateMachine) 
        : base(stateMachine)
        {
            this.allyAI = allyAI;
        }

        public override void Enter()
        {
            base.Enter();
            allyAI.Animator.SetTrigger("Die");
        }

        public override void Exit()
        {
            base.Exit();
            allyAI.Animator.ResetTrigger("Die");
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