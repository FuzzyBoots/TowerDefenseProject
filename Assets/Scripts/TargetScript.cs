using System;
using UnityEngine;

public class TargetScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        EventManager.SubscribeToDamage(gameObject.GetInstanceID(), Damage);
    }

    private void OnDisable()
    {
        EventManager.UnsubscribeToDamage(gameObject.GetInstanceID());
    }

    private void Damage(float damage)
    {
        Debug.Log($"Object {gameObject.name} - Damage for {damage}");
    }
}
