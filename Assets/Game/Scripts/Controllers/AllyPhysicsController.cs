using UnityEngine;
using Game.Core.Army;
using Game.Core;


public class AllyPhysicsController : MonoBehaviour
{
    [SerializeField]
    private AllyAI ally;
    private SphereCollider _collider;
    
    private void Start() {
        _collider = GetComponent<SphereCollider>();
        _collider.radius = ally._AllyData.AttackRange;
    }

    private void OnTriggerEnter(Collider other) 
    {
        IDamageable damageable = other.GetComponent<IDamageable>();

        if (damageable != null)
        {
            if(damageable.gameObject.tag == "Enemy" || damageable.gameObject.tag == "Obstacle")
                ally.AddNewEnemy(damageable);
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        IDamageable damageable = other.GetComponent<IDamageable>();

        if (damageable != null)
        {
            if(ally.CurrentTarget == damageable)
                ally.CurrentTarget = null;
        }
    }
}
