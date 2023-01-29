using UnityEngine;
using Game.Core.Enums;
using Game.Core.Data;
using Game.Core;
using Game.Managers;
using Game.Core.Events;
using EventType = Game.Core.Enums.EventType;

public class Bullet : MonoBehaviour
{
    [SerializeField] public AllyType AllyType;
    [SerializeField] protected TrailRenderer trailRenderer;
    [SerializeField] protected SO_BulletTrail bulletTrailSO;

    protected BulletData _bulletData;
    protected BulletTrailData _bulletTrailData;
    protected Rigidbody _rigidBody;
    protected PoolManager _poolManager;
    protected float _autoDestroyTime;
    protected float _spawnDelay;
    protected float _moveSpeed;
    protected float _damage;
    protected Transform _target;
    private BulletData GetData() => Resources.Load<SO_Bullet>("Data/SO_Bullet").BulletDatas[AllyType];
    private BulletTrailData GetTrailData() => bulletTrailSO.BulletTrailData;

    protected const string DISABLE_METHOD_NAME = "Disable";
    protected const string DO_DISABLE_METHOD_NAME = "DoDisable";
    private const int DISABLED_BULLET_LAYER = 12;
    private void Awake()
    {
        _bulletData = GetData();
        _bulletTrailData = GetTrailData();
        _autoDestroyTime = _bulletData.AutoDestroyTime;
        _spawnDelay = _bulletData.SpawnDelay;
        _moveSpeed = _bulletData.MoveSpeed;
        _damage = _bulletData.Damage;

        _rigidBody = GetComponent<Rigidbody>();
        _poolManager = ManagerProvider.GetManager<PoolManager>();
    }

    public void SetLayer(int newLayer)
    {
        gameObject.layer = newLayer;
    }
    public virtual void Shoot(Quaternion muzzleRotation, Transform target)
    {
        this._target = target;
        transform.rotation = muzzleRotation;
        _rigidBody.AddForce(transform.forward * _moveSpeed, ForceMode.VelocityChange);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageable;
        if (other.TryGetComponent<IDamageable>(out damageable))
        {
            if(GetType() == typeof(HomingBullet))
                EventBase.NotifyListeners(EventType.RocketExploded);

            damageable.TakeDamage(_damage);
            Disable();
        }
    }

    private void ConfigureTrail()
    {
        if(trailRenderer != null && bulletTrailSO != null) 
        {
            _bulletTrailData.SetupTrail(trailRenderer);
        }
    }

    private void Disable()
    {
        CancelInvoke(DISABLE_METHOD_NAME);
        CancelInvoke(DO_DISABLE_METHOD_NAME);

        _rigidBody.velocity = Vector3.zero;
        SetLayer(DISABLED_BULLET_LAYER);

        if(trailRenderer != null && bulletTrailSO != null) 
        {
            //_isDisabling = true;
            Invoke(DO_DISABLE_METHOD_NAME, _bulletTrailData.Time);
        }
        else 
        {
            DoDisable();
        }
    }

    public virtual void DoDisable()
    {
        if(trailRenderer != null && bulletTrailSO != null) 
        {
            trailRenderer.Clear();
        }
        _poolManager.ReturnBulletToPool(this.gameObject);
    }
    private void OnEnable()
    {
        CancelInvoke(DISABLE_METHOD_NAME);
        CancelInvoke(DO_DISABLE_METHOD_NAME);
        Invoke(DISABLE_METHOD_NAME, _autoDestroyTime);
        ConfigureTrail();
        //_isDisabling = false;
    }
}
