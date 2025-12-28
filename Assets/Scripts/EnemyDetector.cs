using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    HashSet<int> _viableTargets = new HashSet<int>();
    bool _attacking = false;

    TurretScript turretScript;

    private void Awake()
    {
        turretScript = GetComponent<TurretScript>();
    }

    private void OnTriggerEnter(Collider other)
    {
        int id = other.GetInstanceID();
        _viableTargets.Add(id);
        TryAttacking(id);
    }

    private void OnTriggerExit(Collider other)
    {
        int id = other.GetInstanceID();
        _viableTargets.Remove(id);
        TryStopAttacking(id);
    }

    private void TryAttacking(int id)
    {
        // If we're already attacking, we return
        if ( _attacking ) { return; }
        _attacking = true;

        // Call the parent turret's attack function
        turretScript.StartAttacking(id);
    }

    private void TryStopAttacking(int id)
    {
        if (!_attacking) { return; }
        _attacking = false;

        // Call the parent turret's function to stop attacking
        Debug.Log("Stopping attacking");
        turretScript.StopAttacking();

        if (_viableTargets.Count > 0)
        {
            // Get the first viable target 
            int newId = _viableTargets.ElementAt(0);
            TryAttacking(newId);
        } 
    }
}
