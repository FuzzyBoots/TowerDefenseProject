using System;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class EventManager
{
    public static Action<int> OnDeath;

    public static Action OnStartPlacement;
    public static Action OnStopPlacement;

    public static Action<int> OnFundsChange;
    public static Action<int> OnPurseChange;

    private static Dictionary<int, Action<float>> _damageSubscriptions = new Dictionary<int, Action<float>>();

    public static void ClearDamageSubscriptions()
    {
        _damageSubscriptions.Clear();
    }

    public static void SubscribeToDamage(int targetID, Action<float> callback)
    {
        _damageSubscriptions[targetID] = callback;
    }

    public static void UnsubscribeToDamage(int targetID)
    {
        _damageSubscriptions.Remove(targetID);
    }

    public static void BroadcastDamage(int targetID, float amount)
    {
        if (_damageSubscriptions.TryGetValue(targetID, out var callback))
        {
            callback?.Invoke(amount);
        }
    }
}
