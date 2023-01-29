using UnityEngine;
using Game.Core.Army;
using Game.Core;
using Game.Core.Vehicle;
using Game.Core.Vehicle.Enemy;
public class EnemyHelicopterPhysicsController : MonoBehaviour
{
    [SerializeField]
    private EnemyHelicopter enemyHelicopter;
    private SphereCollider _collider;
    private bool _playerEntered;
    
    private void Start() {
        _collider = GetComponent<SphereCollider>();
        _collider.radius = enemyHelicopter.InboundRange;
        //_collider.radius = enemyHelicopter._EnemyData.AttackRange;
    }

    private void OnTriggerEnter(Collider other) 
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        
        if (damageable != null)
        {
            if(damageable.gameObject.tag == "AllyVehicle")
            {
                _playerEntered = true;
                
                //enemyHelicopter.canMove = true;
                enemyHelicopter.EncounterStrategy();
            }
                
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        IDamageable damageable = other.GetComponent<IDamageable>();

        if (damageable != null)
        {
            if(damageable.gameObject.tag == "AllyVehicle" && _playerEntered)
            {
                
            }
        }
    }
}
