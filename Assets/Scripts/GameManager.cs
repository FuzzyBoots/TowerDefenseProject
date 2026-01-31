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
    [SerializeField] float _cameraRotationSpeed = 30f;

    [SerializeField] Transform _lookAtPoint;

    Vector2 _movementAmount = Vector2.zero;
    InputAction _inputMove;
    InputAction _inputRotate;
    [SerializeField] private float _cameraChangeSpeed = 0.5f;

    private void Awake()
    {
        _camera = Camera.main;

        _inputMove = InputSystem.actions.FindAction("Move");
        _inputRotate = InputSystem.actions.FindAction("Rotate");
    }

    private void HandleCameraMovement()
    {
        _movementAmount = _inputMove.ReadValue<Vector2>();
        if (_movementAmount != Vector2.zero)
        {
            Vector3 camForward = _camera.transform.forward;
            Vector3 camRight = _camera.transform.right;
            
            camForward.y = 0;
            camRight.y = 0;

            camForward.Normalize();
            camRight.Normalize();

            Vector3 move = camForward * _movementAmount.y + camRight * _movementAmount.x;
            _lookAtPoint.position += _cameraMoveSpeed * Time.deltaTime * move;
        }

        float _cameraRotationAmount = _inputRotate.ReadValue<float>();
        if (_cameraRotationAmount != 0f)
        {
            _lookAtPoint.transform.Rotate(Vector3.up, _cameraRotationAmount * _cameraRotationSpeed * Time.deltaTime);
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

    public bool _placingTurret;

    [SerializeField] GameObject _turretPrefab;
}
