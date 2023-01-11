using UnityEngine;
using Game.Core.Enums;
using Game.Core.Data;
using Game.Core;
using Game.Managers;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    public AllyType AllyType;
    [SerializeField] private TrailRenderer trailRenderer;
    private BulletData _bulletData;
    private Rigidbody _rigidBody;
    private PoolManager _poolManager;
    private float _autoDestroyTime;
    private float _spawnDelay;
    private float _moveSpeed;
    public float _damage;
    private BulletData GetData() => Resources.Load<SO_Bullet>("Data/SO_Bullet").BulletDatas[AllyType];
    private void Awake()
    {
        _bulletData = GetData();
        _autoDestroyTime = _bulletData.AutoDestroyTime;
        _spawnDelay = _bulletData.SpawnDelay;
        _moveSpeed = _bulletData.MoveSpeed;
        _damage = _bulletData.Damage;

        _rigidBody = GetComponent<Rigidbody>();
        _poolManager = ManagerProvider.GetManager<PoolManager>();
    }
    public void Shoot(Quaternion muzzleRotation)
    {
        transform.rotation = muzzleRotation;
        _rigidBody.AddForce(transform.forward * _moveSpeed, ForceMode.VelocityChange);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageable;
        if (other.TryGetComponent<IDamageable>(out damageable))
        {
            damageable.TakeDamage(_damage);
            Disable();
        }
    }

    private void Disable()
    {
        
        CancelInvoke("Disable");
        _rigidBody.velocity = Vector3.zero;
        trailRenderer.Clear();
        _poolManager.ReturnBulletToPool(this.gameObject);
    }
    private void OnEnable()
    {
        CancelInvoke("Disable");
        Invoke("Disable", _autoDestroyTime);
    }
}
