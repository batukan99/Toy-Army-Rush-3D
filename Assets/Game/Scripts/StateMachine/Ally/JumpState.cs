
using System.Collections.Generic;
using UnityEngine;
using Game.Core.Army;

namespace Game.StateMachines.Ally  
{
    public class JumpState : State
    {
        public AllyAI allyAI;
        public JumpState(AllyAI allyAI, ArmyStateMachine stateMachine) 
        : base(stateMachine)
        {
            this.allyAI = allyAI;
        }

        public override void Enter()
        {
            base.Enter();
            allyAI.Animator.SetTrigger("Jump");
        }

        public override void Exit()
        {
            base.Exit();
            allyAI.Animator.ResetTrigger("Jump");
        }

        public override void HandleInput()
        {
            base.HandleInput();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (!allyAI.Animator.GetCurrentAnimatorStateInfo(0).IsName("Jump") && !allyAI.IsJumpedIntoVehicle) 
            {
                //allyAI.IsJumpedIntoVehicle = true;
            }

        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
    }

}