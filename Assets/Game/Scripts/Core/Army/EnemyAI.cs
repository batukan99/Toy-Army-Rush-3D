using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Game.StateMachines;
using Game.StateMachines.Enemy;
using Game.Core.Data;
using Game.Controllers;
using Game.Core.Vehicle;
using Game.Core.Events;
using EventType = Game.Core.Enums.EventType;

namespace Game.Core.Army 
{
    public abstract class EnemyAI : IDamageable
    {
        [SerializeField]
        private EnemyAttackController attackController;
        public abstract EnemyData GetEnemyData();
        public abstract void SetEnemyData();
        public EnemyData _EnemyData;
        public Collider Collider { get; private set; }

        public Animator Animator { get; private set; }

        public ArmyStateMachine armySM;

        public IDamageable CurrentTarget;
        public VehicleBase VehicleTarget;

        [HideInInspector] public bool AimedAtTarget = false;
        public List<IDamageable> allies = new List<IDamageable>();

        public IdleState idleState;
        public AimState aimState;
        public AttackState attackState;
        public DeathState deathState;

        #region Private
        public float _health = 100;
        private int _damage;
        private float _aimRange;
        private float _attackRange;

        public bool _isAlive = true;
        private bool _isDeath = false;
        public bool _isGameOver = false;
        private bool _status = false;
        #endregion

        #region MonoBehaviour Callbacks
        private void Awake()
        {
            SetEnemyData();
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
            aimState = new AimState(this, armySM);
            attackState = new AttackState(this, armySM, attackController);
            deathState = new DeathState(this, armySM);

            
            At(idleState, aimState, CanAim());
            At(aimState, attackState, IsInAttackRange());
            At(attackState, aimState, IfTargetNotValid());
            At(aimState, deathState, IsDeath());
            At(deathState, idleState, IsAlive());

            Any(deathState, IsDeath());
            Any(idleState, IsPlayerWon());  //play lost dance
            Any(idleState, IsPlayerLost());  //play won dance

            armySM.ChangeState(idleState);

            void At(State from, State to, Func<bool> condition) => armySM.AddTransition(from, to, condition);
            void Any(State to, Func<bool> condition) => armySM.AddAnyTransition(to, condition);


            Func<bool> CanAim() => () => !_isDeath  && !AimedAtTarget;
            Func<bool> IsInAttackRange() => () => !_isDeath && CurrentTarget != null && CurrentTarget.IsAttackable()
                && AimedAtTarget && IsInRange();
            Func<bool> IfTargetNotValid() => () => !_isDeath && 
                (!AimedAtTarget || ( CurrentTarget!= null && (CurrentTarget.IsDeath() || !IsInRange()) ) || IsAtBehind());
            
            Func<bool> IsDeath() => () => _health <= 0;
            Func<bool> IsAlive() => () => _health > 0;
            Func<bool> IsPlayerWon() => () => _isGameOver && _status;
            Func<bool> IsPlayerLost() => () => _isGameOver && !_status;
            bool IsInRange() => Vector3.Distance(transform.position, CurrentTarget.transform.position) <= _EnemyData.AttackRange && !IsAtBehind();
            bool IsAtBehind() => GetTransform().position.z < Camera.main.transform.position.z;
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

        public void AddNewAlly(AllyAI allyAI) 
        {
            allies.Add(allyAI);
        }
        public bool DisabledDamageables(IDamageable damageable)
        {
            return damageable != null && (damageable.IsDeath() 
              //  || Vector3.Distance(GetTransform().position, CurrentTarget.GetTransform().position) > _EnemyData.AttackRange
                );
        }
        public IDamageable GetClosestDamageable()
        {
            float closestDistance = float.MaxValue;

            IDamageable closestDamageable = null;
            
            for (int i = 0; i < allies.Count; i++)
            {
                if(!allies[i].IsAttackable())
                    continue;
                Transform damageableTransform = allies[i].GetTransform();
                float distance = Vector3.Distance(transform.position, damageableTransform.position);

                if (distance < closestDistance)
                {
                    closestDamageable = allies[i];
                }
            }
            if(closestDamageable == null && VehicleTarget != null) 
            {
                closestDamageable = VehicleTarget;
            }
            
            return closestDamageable;
        }

        private void OnDeath()
        {
            _isDeath = true;
        }
        private void OnGameOver(bool status) 
        {
            _isGameOver = true;
            this._status = status;
        }
        public void SetTargetAllyArmy(IDamageable target)
        {
            CurrentTarget = (AllyAI)target;
        }

        public override void TakeDamage(float damage)
        {
            _health -= damage;
            if (_health <= 0)
                OnDeath();
        }

        public override Transform GetTransform() => transform;

        public override bool IsAttackable() => !IsDeath();

        public override bool IsDeath() => _isDeath;
        #endregion

        private void OnEnable()
        {
            EventBase.StartListening(EventType.GameOver, (UnityAction<bool>)OnGameOver);
        }

        private void OnDisable()
        {
            EventBase.StopListening(EventType.GameOver, (UnityAction<bool>)OnGameOver);
        }

    }

}
