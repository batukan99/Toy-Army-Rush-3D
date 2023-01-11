using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core.Army;

namespace Game.StateMachines
{
    public class State 
    {
        protected ArmyStateMachine stateMachine;

        protected State(ArmyStateMachine stateMachine) 
        {
            this.stateMachine = stateMachine;
        }

        public virtual void Enter()
        {

        }

        public virtual void HandleInput()
        {

        }
        public virtual void LogicUpdate()
        {

        }
        public virtual void PhysicsUpdate()
        {

        }
        public virtual void Exit()
        {

        }
        
    }

}
