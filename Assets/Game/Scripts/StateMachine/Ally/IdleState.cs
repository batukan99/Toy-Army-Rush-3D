using System.Collections;
using System.Collections.Generic;
using Game.Core.Army;
using UnityEngine;

namespace Game.StateMachines.Ally 
{
    public class IdleState : State
    {
        public AllyAI allyAI;
        public IdleState(AllyAI allyAI, ArmyStateMachine stateMachine) 
        : base(stateMachine)
        {
            this.allyAI = allyAI;
        }

        public override void Enter()
        {
            base.Enter();
            allyAI.Animator.SetTrigger("Idle");
        }

        public override void Exit()
        {
            base.Exit();
            allyAI.Animator.ResetTrigger("Idle");
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
