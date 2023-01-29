using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.StateMachines.Ally;
using Game.StateMachines;
using Game.Core.Data;
using Game.Controllers;
using Game.Core.Events;
using EventType = Game.Core.Enums.EventType;
using DG.Tweening;

namespace Game.Core.Army 
{
    public abstract class AllyAI : IDamageable
    {
        [SerializeField]
        private AllyAttackController attackController;
        public abstract AllyData GetAllyData();
        public abstract void SetAllyData();
        public AllyData _AllyData;
        public Collider Collider { get; private set; }

        public Animator Animator { get; private set; }
        [SerializeField] public SkinnedMeshRenderer meshRenderer;
        [SerializeField] private Color deathColor = new Color32(80, 80, 80, 255);

        public ArmyStateMachine armySM;

        public IDamageable CurrentTarget;
        public State CurrentState;
        public List<IDamageable> enemies = new List<IDamageable>();
        
        [HideInInspector] public bool IsJumpedIntoVehicle = false;
        [HideInInspector] public bool IsPlaced = false;
        [HideInInspector] public bool IsShifted = false;
        [HideInInspector] public float LastShiftedValue = 0;
        [HideInInspector] public bool AimedAtTarget = false;
        [HideInInspector] public int PieceIndex = 0;

        public IdleState idleState;
        public JumpState jumpState;
        public AimState aimState;
        public AttackState attackState;
        public DeathState deathState;

        #region Private
        public float _health = 100;
        public int _damage;
        private float _aimRange;
        //public float _attackRange => ;

        private bool _isAlive = true;
        private bool _isDeath = false;
        

        #endregion

        #region MonoBehaviour Callbacks
        private void Awake()
        {
            SetAllyData();
            Collider = GetComponent<Collider>();
            Animator = GetComponentInChildren<Animator>();
        }

        private void Start()
        {
            InitArmy();
        }
        private void InitArmy() 
        {
            armySM = new ArmyStateMachine();

            idleState = new IdleState(this, armySM);
            jumpState = new JumpState(this, armySM);
            aimState = new AimState(this, armySM);
            attackState = new AttackState(this, armySM, attackController);
            deathState = new DeathState(this, armySM);

            At(idleState, jumpState, IsAbleToJump());
            At(jumpState, aimState, CanAim());
            At(aimState, attackState, IsInAttackRange());
            At(attackState, aimState, IfTargetNotValid());
            At(aimState, deathState, IsDeath());
            At(deathState, idleState, IsAlive());

            Any(deathState, IsDeath());

            armySM.ChangeState(idleState);

            void At(State from, State to, Func<bool> condition) => armySM.AddTransition(from, to, condition);
            void Any(State to, Func<bool> condition) => armySM.AddAnyTransition(to, condition);


            Func<bool> IsAbleToJump() => () => !_isDeath && IsPlaced && !IsJumpedIntoVehicle;
            Func<bool> CanAim() => () => !_isDeath  && IsJumpedIntoVehicle && !AimedAtTarget;
            Func<bool> IsInAttackRange() => () => !_isDeath && CurrentTarget != null && CurrentTarget.IsAttackable() 
                && AimedAtTarget && IsInRange();
            Func<bool> IfTargetNotValid() => () => !_isDeath && IsJumpedIntoVehicle && 
                (!AimedAtTarget || ( CurrentTarget!= null && (CurrentTarget.IsDeath() || !IsInRange()) ) || IsEnemyAtBehind());
            
            Func<bool> IsDeath() => () => _health <= 0;
            Func<bool> IsAlive() => () => _health > 0;
            bool IsInRange() => Vector3.Distance(transform.position, CurrentTarget.transform.position) <= _AllyData.AttackRange && !IsEnemyAtBehind();
            bool IsEnemyAtBehind() => CurrentTarget != null && CurrentTarget.GetTransform().position.z < Camera.main.transform.position.z;
        }

        private void Update()
        {
            if(!_isAlive) 
                return;
 
            //armySM.CurrentState.HandleInput();
            armySM.LogicUpdate();
        }

        private void FixedUpdate()
        {
            armySM.CurrentState.PhysicsUpdate();
        }
        #endregion

        #region Methods
        public void AddNewEnemy(IDamageable damageable) 
        {
            enemies.Add(damageable);
        }
        public bool DisabledDamageables(IDamageable damageable)
        {
            return damageable != null && (damageable.IsDeath() 
                || Vector3.Distance(GetTransform().position, damageable.GetTransform().position) > _AllyData.AttackRange);
        }
        public IDamageable GetClosestDamageable()
        {
            float closestDistance = float.MaxValue;

            IDamageable closestDamageable = null;
            
            for (int i = 0; i < enemies.Count; i++)
            {
                Transform damageableTransform = enemies[i].GetTransform();
                float distance = Vector3.Distance(transform.position, damageableTransform.position);

                if (distance < closestDistance)
                {
                    closestDamageable = enemies[i];
                }
            }
            
            return closestDamageable;
        }

        public void Kill()
        {
            _health = 0;
            _isDeath = true;
            meshRenderer.material.DOColor(deathColor, 1.5f).SetEase(Ease.OutQuart);
            EventBase.NotifyListeners(EventType.AllyDied, (AllyAI)this);
        }
        public void SetTargetEnemyArmy(IDamageable target)
        {
            CurrentTarget = (EnemyAI)target;
        }
        public override void TakeDamage(float damage)
        {
            _health -= damage;
            if (_health <= 0)
                Kill();
        }

        public override Transform GetTransform() => transform;

        public override bool IsAttackable() => !IsDeath() && IsJumpedIntoVehicle;

        public override bool IsDeath() => _isDeath;

        #endregion


    }


}
