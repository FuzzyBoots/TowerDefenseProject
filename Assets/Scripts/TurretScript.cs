using System;
using System.Collections;
using UnityEngine;

public class TurretScript : MonoBehaviour
{
    Coroutine attackRoutine;
    // For the first approximation, we'll just have the turret firing intermittently

    [SerializeField] float _attackInterval = 1f;

    internal void StartAttacking(int id)
    {
        Debug.Log($"Starting to attack {id}");
        if (attackRoutine == null)
            attackRoutine = StartCoroutine(DoFiring(id));
        else
            Debug.Log("Except already attacking!");
    }

    private IEnumerator DoFiring(int id)
    {
        WaitForSeconds attackInterval = new WaitForSeconds(_attackInterval);
        while (true)
        {
            Debug.Log($"Firing at {id}");
            EventManager.BroadcastDamage(id, 15f);
            yield return attackInterval;
        }
    }

    internal void StopAttacking()
    {
        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
            attackRoutine = null;
        }
    }
}
