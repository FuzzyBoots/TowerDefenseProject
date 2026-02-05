using Cinemachine;
using System;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlacementManager : MonoBehaviour
{
    [SerializeField] GameObject _currentPrefab;
    [SerializeField] LayerMask _placementLayer;

    private void Start()
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
            if (Physics.Raycast(ray, out raycastHit, 100f, _placementLayer))
            {
                Collider targetCollider = raycastHit.collider;
                float topY = targetCollider.bounds.max.y;

                turretPosition = new(raycastHit.point.x, topY, raycastHit.point.z);
                _currentPrefab.transform.position = turretPosition;
                Debug.Log("Hit " + raycastHit.collider.name);
            }
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            // Place the turret at turretPosition and activate it
        }

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            CancelSetting();
        }
    }

    public void SetPrefab(GameObject prefab)
    {
        _currentPrefab = Instantiate(prefab);
        Debug.Log("Instantiated Prefab " + prefab.name);
    }

    public void CancelSetting()
    {
        Destroy(_currentPrefab);
    }
}
