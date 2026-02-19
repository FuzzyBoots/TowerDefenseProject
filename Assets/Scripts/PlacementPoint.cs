using System;
using UnityEngine;
using UnityEngine.Assertions;

public class PlacementPoint : MonoBehaviour
{
    [SerializeField] ParticleSystem _beacon;
    [SerializeField] TurretScript _turret;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public bool IsOccupied => _turret != null;

    void Start()
    {
        if (_beacon == null)
        {
            _beacon = GetComponentInChildren<ParticleSystem>();
        }
        Assert.IsNotNull(_beacon, "PlacementPoint requires a ParticleSystem as a beacon.");

        EventManager.OnStartPlacement += EnablePlacement;
        EventManager.OnStopPlacement += DisablePlacement;
    }

    private void DisablePlacement()
    {
        _beacon.Stop();
    }

    private void EnablePlacement()
    {
        if (_turret == null)
        {
            _beacon.Play();
        }
    }

    public void SetTurret(TurretScript turret)
    {
        _turret = turret;
        _beacon.Stop();
    }

    public void ClearTurret()
    {
        _turret = null;
        _beacon.Play();
    }
}
