using UnityEngine;
using Game.Core.Army;
using Game.Core;
using Game.Core.Vehicle;

public class EnemyPhysicsController : MonoBehaviour
{
    [SerializeField]
    private EnemyAI enemyAI;
    private SphereCollider _collider;
    
    private void Start() {
        _collider = GetComponent<SphereCollider>();
        _collider.radius = enemyAI._EnemyData.AttackRange;
    }

    private void OnTriggerEnter(Collider other) 
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        
        if (damageable != null)
        {
            //if(damageable.gameObject.tag == "Ally")
            //    enemyAI.AddNewAlly((AllyAI)damageable);
            if(damageable.gameObject.tag == "AllyVehicle")
                enemyAI.VehicleTarget = (VehicleBase)damageable;
                
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        IDamageable damageable = other.GetComponent<IDamageable>();

        if (damageable != null)
        {
            if(enemyAI.CurrentTarget == damageable)
                enemyAI.CurrentTarget = null;
        }
    }
}
