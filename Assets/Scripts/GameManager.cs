using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    Camera _camera;
    [SerializeField] LayerMask _enemyLayers;
    [SerializeField] LayerMask _turretLayers;
    [SerializeField] GameObject _obstaclePrefab;

    [SerializeField] float _cameraMoveSpeed = 5f;

    Vector2 _movementAmount = Vector2.zero;
    InputAction _inputMove;
    [SerializeField] private float _cameraChangeSpeed = 0.5f;

    private void Awake()
    {
        _camera = Camera.main;

        _inputMove = InputSystem.actions.FindAction("Move");
        //_inputMove.performed += HandleCameraMovement;
        //_inputMove.canceled += CameraMovementEnded;
    }

    private void CameraMovementEnded(InputAction.CallbackContext context)
    {
        Debug.Log("Dead stick?");
        Debug.Log(context.ReadValue<Vector2>());
    }

    private void HandleCameraMovement()
    {
        _movementAmount = _inputMove.ReadValue<Vector2>();
        if (_movementAmount != Vector2.zero)
        {
            Debug.Log("Moving camera by: " + _movementAmount);
            Vector3 move = new Vector3(_movementAmount.x, 0, _movementAmount.y);
            _camera.transform.position += move * _cameraMoveSpeed * Time.deltaTime;
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleCameraMovement();

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _enemyLayers))
            {
                if (hit.collider.gameObject.TryGetComponent<TargetScript>(out TargetScript target))
                {
                    target.Kill(_obstaclePrefab);
                }
            }
            if (Physics.Raycast(ray, out RaycastHit turretHit, Mathf.Infinity, _turretLayers))
            {
                if (turretHit.collider.gameObject.TryGetComponent<TurretScript>(out TurretScript turret))
                {
                    Debug.Log("Tagged Turret: " + turretHit.collider.gameObject.name);
                }
            }
        }
    }
}
