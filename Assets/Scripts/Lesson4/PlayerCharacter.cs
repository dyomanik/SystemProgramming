using System.Collections.Generic;
using UnityEngine;

namespace Lesson4
{
    public class PlayerCharacter : Character
    {
        [Range(0, 100)][SerializeField] private int _health = 100;
        [Range(0.5f, 10.0f)][SerializeField] private float _movingSpeed = 8.0f;
        [Range(0.5f, 1.0f)][SerializeField] private float _rotationSpeed = 1.0f;
        [SerializeField] private float _acceleration = 3.0f;
        private const float _gravity = -9.8f;
        private CharacterController _characterController;
        private MouseLook _mouseLook;
        private Vector3 _currentVelocity;
        private List<Transform> _spawnTransforms = new List<Transform>();
        protected override FireAction fireAction { get; set; }
        protected override void Initiate()
        {
            base.Initiate();
            fireAction = gameObject.AddComponent<RayShooter>();
            fireAction.Reloading();
            _characterController = GetComponentInChildren<CharacterController>();
            _characterController ??= gameObject.AddComponent<CharacterController>();
            _mouseLook = GetComponentInChildren<MouseLook>();
            _mouseLook ??= gameObject.AddComponent<MouseLook>();
        }
        public override void Movement()
        {
            if (_mouseLook != null && _mouseLook.PlayerCamera != null)
            {
                _mouseLook.PlayerCamera.enabled = IsOwner;
            }
            if (IsOwner)
            {
                var moveX = Input.GetAxis("Horizontal") * _movingSpeed;
                var moveZ = Input.GetAxis("Vertical") * _movingSpeed;
                var movement = new Vector3(moveX, 0, moveZ);
                movement = Vector3.ClampMagnitude(movement, _movingSpeed);
                movement *= Time.deltaTime;
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    movement *= _acceleration;
                }
                movement.y = _gravity;
                movement = transform.TransformDirection(movement);
                _characterController.Move(movement);
                _mouseLook.Rotation();
                UpdatePositionServerRpc(transform.position);
                UpdateRotationServerRpc(transform.rotation);
            }
            else
            {
                transform.position = Vector3.SmoothDamp(transform.position, serverPosition.Value, ref _currentVelocity, _movingSpeed * Time.deltaTime);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, serverRotation.Value, _rotationSpeed);
            }
        }
        private void Start()
        {
            
            Initiate();
        }
        private void OnGUI()
        {
            if (Camera.main == null)
            {
                return;
            }
            var info = $"Health: {_health}\nClip: {fireAction.BulletCount}";
            var size = 12;
            var bulletCountSize = 50;
            var posX = Camera.main.pixelWidth / 2 - size / 4;
            var posY = Camera.main.pixelHeight / 2 - size / 2;
            var posXBul = Camera.main.pixelWidth - bulletCountSize * 2;
            var posYBul = Camera.main.pixelHeight - bulletCountSize;
            GUI.Label(new Rect(posX, posY, size, size), "+");
            GUI.Label(new Rect(posXBul, posYBul, bulletCountSize * 2,
            bulletCountSize * 2), info);
        }

        public override void OnNetworkSpawn()
        {
            Initiate();
            var spawnMarkers = FindObjectsOfType<SpawnPointMarker>();
            foreach (var spawnPointMarker in spawnMarkers)
            {
                _spawnTransforms.Add(spawnPointMarker.transform);
            }
            if (_spawnTransforms.Count > 0)
            {
                transform.position = _spawnTransforms[Random.Range(0, _spawnTransforms.Count)].position;
            }
            base.OnNetworkSpawn();
            
        }
    }
}