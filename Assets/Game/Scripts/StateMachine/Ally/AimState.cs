
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Game.Core.Army;
using DG.Tweening;

namespace Game.StateMachines.Ally 
{
    public class AimState : State
    {
        public AllyAI allyAI;
        private IDamageable _target;
        public AimState(AllyAI allyAI, ArmyStateMachine stateMachine) 
        : base(stateMachine)
        {
            this.allyAI = allyAI;
        }

        public override void Enter()
        {
            base.Enter();
            allyAI.Animator.SetTrigger("Aim");
        }

        public override void Exit()
        {
            base.Exit();
            allyAI.Animator.ResetTrigger("Aim");
        }

        public override void HandleInput()
        {
            base.HandleInput();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            if(allyAI.CurrentTarget != null)
            {
                allyAI.enemies.RemoveAll(allyAI.DisabledDamageables);
                if(allyAI.CurrentTarget.IsDeath()) 
                {
                    allyAI.CurrentTarget = null;
                }     
            }

            _target = allyAI.GetClosestDamageable();
            
            if(_target != null) 
            {
                allyAI.CurrentTarget = _target;
                allyAI.AimedAtTarget = true;
                //allyAI.GetTransform().DOLookAt(enemy.GetTransform().position, 0.25f).OnComplete(
                //    () => allyAI.AimedAtTarget = true);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
       
    }

}