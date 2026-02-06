using Cinemachine;
using nTools.PrefabPainter;
using System;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlacementManager : MonoBehaviour
{
    [SerializeField] GameObject _currentPrefab;
    [SerializeField] LayerMask _placementLayer;
    private bool _overPlacement;

    private void Awake()
    {
        CinemachineCore.CameraUpdatedEvent.AddListener(CameraUpdate);
    }

    private void CameraUpdate(CinemachineBrain arg0)
    {
        Vector3 turretPosition;

        if (_currentPrefab != null)
        {
            Vector3 mousePosition = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);

            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red);

            RaycastHit raycastHit;
            if (Physics.Raycast(ray, out raycastHit, 100f, _placementLayer, QueryTriggerInteraction.Collide))
            {
                Collider targetCollider = raycastHit.collider;
                // Different behavior for a Placement Point or a Wall/Floor
                if (targetCollider.gameObject.layer == LayerMask.NameToLayer("Placement"))
                {
                    Debug.Log("Placement at " + targetCollider.transform.position);
                    _overPlacement = true;
                    _currentPrefab.transform.position = targetCollider.transform.position;
                } else
                {
                    _overPlacement = false;
                    float topY = targetCollider.bounds.max.y;

                    turretPosition = new(raycastHit.point.x, topY, raycastHit.point.z);
                    _currentPrefab.transform.position = turretPosition;
                }                    
            }
        }

        if (Mouse.current.leftButton.wasPressedThisFrame && !IsCursorOverUI())
        {
            Debug.Log("Clicked");
            if (_overPlacement)
            {
                Debug.Log("Placed?");
                // Place the turret at turretPosition and activate it
                // We'll assign a new copy to the one we're moving... will this work?
                _currentPrefab = Instantiate(_currentPrefab);
            }
        }

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            Debug.Log("Canceled");
            CancelSetting();
        }
    }

    public void SetPrefab(GameObject prefab)
    {
        if (_currentPrefab != null)
        {
            Destroy(_currentPrefab);
        }

        _currentPrefab = Instantiate(prefab);
    }

    public void CancelSetting()
    {
        if (_currentPrefab)
        {
            Destroy(_currentPrefab);
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
