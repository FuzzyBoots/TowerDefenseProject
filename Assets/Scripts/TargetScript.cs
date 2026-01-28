using System;
using UnityEngine;

public class TargetScript : MonoBehaviour
{
    int _entityID;
    float _health = 50f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _entityID = gameObject.GetEntityId();
        EventManager.SubscribeToDamage(_entityID, Damage);
        TargetDatabase.RegisterTarget(_entityID, this);
    }

    private void OnDisable()
    {
        EventManager.UnsubscribeToDamage(_entityID);
        TargetDatabase.UnregisterTarget(_entityID);
    }

    private void Damage(float damage)
    {
        Debug.Log($"Object {gameObject.name} - Damage for {damage}");
        _health -= damage;
        if (_health <= 0)
        {
            Debug.Log($"Object {gameObject.name} - Died");
            EventManager.OnDeath?.Invoke(_entityID);
            gameObject.SetActive(false);
        }
    }

    public int EntityID()
    {
        return _entityID;
    }

    internal void Kill(GameObject obstaclePrefab = null)
    {
        if (obstaclePrefab)
        {
            // Spawn a NavMeshObstacle
            GameObject obstacle = Instantiate(obstaclePrefab);
            obstacle.transform.position = transform.position;
            // Debug.Break();
        }

        // Destroy yourself.
        Destroy(gameObject, 0.1f);
    }
}
