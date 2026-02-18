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
<<<<<<< HEAD
    [SerializeField] float _placementCost;
=======
>>>>>>> 87257ea634e2d3b76102044d9aef7a198090c5dd
    [SerializeField] LayerMask _placementLayer;
    private bool _overPlacement;
    PlacementPoint _placementPoint;

    private void Awake()
    {
        CinemachineCore.CameraUpdatedEvent.AddListener(CameraUpdate);
    }

    private void CameraUpdate(CinemachineBrain arg0)
    {
        Vector3 turretPosition;
        Vector3 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        if (_currentTurret != null)
        {
            
            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red);

            if (Physics.Raycast(ray, out RaycastHit raycastHit, 100f, _placementLayer, QueryTriggerInteraction.Collide))
            {
                Collider targetCollider = raycastHit.collider;
                // Different behavior for a Placement Point or a Wall/Floor
                if (targetCollider.gameObject.layer == LayerMask.NameToLayer("Placement"))
                {
<<<<<<< HEAD
                    // TODO: We need to check to be sure the Placement Point is not already occupied...
                    // maybe it should be disabled when occupied?
=======
                    _placementPoint = targetCollider.GetComponent<PlacementPoint>();
                    Debug.Log("Placement at " + targetCollider.transform.position);
>>>>>>> 87257ea634e2d3b76102044d9aef7a198090c5dd
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

<<<<<<< HEAD
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
=======
            if (Mouse.current.leftButton.wasPressedThisFrame && !IsCursorOverUI())
            {
                if (_overPlacement)
                {
                    // Place the turret at turretPosition and activate it
                    // We'll assign a new copy to the one we're moving... will this work?
                    _currentTurret = Instantiate(_currentTurret);
                    _placementPoint.SetTurret(_currentTurret);
>>>>>>> 87257ea634e2d3b76102044d9aef7a198090c5dd
                }
            }

            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                Debug.Log("Canceled");
                CancelSetting();
            }
        } else
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                if (Physics.Raycast(ray, out RaycastHit raycastHit, 100f, LayerMask.GetMask("Placement"), QueryTriggerInteraction.Collide))
                {
                    // Bring up upgrade dialog
                    Debug.Log("Upgade Dialog");
                }
            }
        }
    }

<<<<<<< HEAD
    public void SetPrefab(TurretScript turret)
=======
    public void SetPrefab(TurretScript prefab)
>>>>>>> 87257ea634e2d3b76102044d9aef7a198090c5dd
    {
        if (_currentTurret != null)
        {
            Destroy(_currentTurret.gameObject);
        }

<<<<<<< HEAD
        _currentTurret = Instantiate(turret);
=======
        EventManager.OnStartPlacement?.Invoke();

        _currentTurret = Instantiate(prefab);
>>>>>>> 87257ea634e2d3b76102044d9aef7a198090c5dd
    }

    public void CancelSetting()
    {
        if (_currentTurret)
        {
            Destroy(_currentTurret.gameObject);
        }

        EventManager.OnStopPlacement?.Invoke();
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
