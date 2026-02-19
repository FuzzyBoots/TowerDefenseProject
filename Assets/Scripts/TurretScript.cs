using System;
using System.Collections;
using UnityEngine;

public class TurretScript : MonoBehaviour
{
    Coroutine attackRoutine;

    [SerializeField] TurretSO _turretData;
    
    [SerializeField] ParticleSystem _firingParticles;

    private float _readyToFire = 0;

    private void Awake()
    {
        if (!_firingParticles)
            _firingParticles = GetComponent<ParticleSystem>();
    }

    public TurretSO GetTurretData() { return _turretData; }

    internal void StartAttacking(int id)
    {
        Debug.Log($"Starting to attack {id}");
        TargetScript target = TargetDatabase.GetTarget(id);

        if (attackRoutine == null)
        {
            attackRoutine = StartCoroutine(DoFiring(target));
        }
        else
        {
            Debug.Log("Except already attacking!");
        }
    }

    private IEnumerator DoFiring(TargetScript target)
    {
        GameObject targetObject = target.gameObject;

        while (true)
        {
            // Rotate toward the target
            Vector3 directionToTarget = (targetObject.transform.position - transform.position).normalized;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, directionToTarget, _turretData._rotationSpeed * Time.deltaTime, 0);
            transform.rotation = Quaternion.LookRotation(newDirection);

            if (_readyToFire <= Time.time && Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, _turretData._attackRange))
            {
                if (hitInfo.collider.gameObject != targetObject)
                {
                    // Target is obstructed
                    Debug.Log($"Target {targetObject.name} obstructed by {hitInfo.collider.gameObject.name}");
                    yield return null;
                    continue;
                } else
                {
                    Debug.Log($"Firing at {targetObject.name} - {target.EntityID()}");
                    _firingParticles.Play();
                    EventManager.BroadcastDamage(target.EntityID(), _turretData._attackDamage);
                    _readyToFire = Time.time + _turretData._attackInterval;
                }
            }
            else
            {
                // Target is out of range
                Debug.Log($"Target {targetObject.name} is out of range");
                yield return null;
                continue;
            }
            
            yield return null;
        }
    }

    public void StopAttacking()
    {
        if (attackRoutine != null)
        {
            _firingParticles.Stop();
            StopCoroutine(attackRoutine);
            attackRoutine = null;
        }
    }

    public void Activate()
    {
        // We should make sure that it starts detecting and attacking things.
    }
}
