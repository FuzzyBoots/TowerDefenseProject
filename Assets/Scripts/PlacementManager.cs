using Cinemachine;
using nTools.PrefabPainter;
using System;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlacementManager : MonoBehaviour
{
    [SerializeField] TurretScript _currentTurret;
    [SerializeField] float _placementCost;
    [SerializeField] LayerMask _placementLayer;
    private bool _overPlacement;

    private void Awake()
    {
        CinemachineCore.CameraUpdatedEvent.AddListener(CameraUpdate);
    }

    private void CameraUpdate(CinemachineBrain arg0)
    {
        Vector3 turretPosition;

        if (_currentTurret != null)
        {
            Vector3 mousePosition = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);

            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red);

            if (Physics.Raycast(ray, out RaycastHit raycastHit, 100f, _placementLayer, QueryTriggerInteraction.Collide))
            {
                Collider targetCollider = raycastHit.collider;
                // Different behavior for a Placement Point or a Wall/Floor
                if (targetCollider.gameObject.layer == LayerMask.NameToLayer("Placement"))
                {
                    // TODO: We need to check to be sure the Placement Point is not already occupied...
                    // maybe it should be disabled when occupied?
                    _overPlacement = true;
                    _currentTurret.transform.position = targetCollider.transform.position;
                }
                else
                {
                    _overPlacement = false;
                    float topY = targetCollider.bounds.max.y;

                    turretPosition = new(raycastHit.point.x, topY, raycastHit.point.z);
                    _currentTurret.transform.position = turretPosition;
                }
            }
        }

        if (Mouse.current.leftButton.wasPressedThisFrame && !IsCursorOverUI())
        {
            if (_overPlacement)
            {
                if (_currentTurret.GetTurretData()._cost <= MoneyManager.Instance.GetWarFunds())
                {
                    // Should this be handled as part of a general validity check?

                    // Place the turret at turretPosition and activate it
                    // We'll assign a new copy to the one we're moving... will this work?
                    _currentTurret = Instantiate(_currentTurret);
                    MoneyManager.Instance.DeductFunds(_currentTurret.GetTurretData()._cost);
                }
            }
        }

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            Debug.Log("Canceled");
            CancelSetting();
        }
    }

    public void SetPrefab(TurretScript turret)
    {
        if (_currentTurret != null)
        {
            Destroy(_currentTurret.gameObject);
        }

        _currentTurret = Instantiate(turret);
    }

    public void CancelSetting()
    {
        if (_currentTurret)
        {
            Destroy(_currentTurret.gameObject);
        }
    }

    private bool IsCursorOverUI()
    {
        // For mouse input: no parameters needed
        if (EventSystem.current.IsPointerOverGameObject())
            return true;

        // For touch input: pass the touch ID (0 for single touch)
        foreach (Touch touch in Input.touches)
        {
            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                return true;
        }

        return false;
    }
}
