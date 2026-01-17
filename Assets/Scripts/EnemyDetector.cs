using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    HashSet<int> _viableTargets = new HashSet<int>();
    bool _attacking = false;
    int _currentTarget;

    TurretScript turretScript;

    private void Awake()
    {
        turretScript = GetComponent<TurretScript>();

        EventManager.OnDeath += RemoveTarget;
    }

    private void OnTriggerEnter(Collider other)
    {
        int id = other.gameObject.GetEntityId();
        _viableTargets.Add(id);
        TryAttacking(id);
    }

    private void OnTriggerExit(Collider other)
    {
        int id = other.gameObject.GetEntityId();
        RemoveTarget(id);
    }

    private void RemoveTarget(int id)
    {
        _viableTargets.Remove(id);
        TryStopAttacking(id);
    }

    private void TryAttacking(int id)
    {
        // If we're already attacking, we return
        if ( _attacking ) { return; }
        _attacking = true;
        _currentTarget = id;

        // Call the parent turret's attack function
        turretScript.StartAttacking(id);
    }

    private void TryStopAttacking(int id)
    {
        if (!_attacking) { return; }
        if (_currentTarget != id) { return; }

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
