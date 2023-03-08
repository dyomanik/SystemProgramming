using System;
using Unity.Netcode;
using UnityEngine;

namespace Lesson4
{
    [RequireComponent(typeof(CharacterController))]
    public abstract class Character : NetworkBehaviour
    {
        protected Action OnUpdateAction { get; set; }
        protected abstract FireAction fireAction { get; set; }
        protected NetworkVariable<Vector3> serverPosition = new NetworkVariable<Vector3>();
        protected NetworkVariable<Quaternion> serverRotation = new NetworkVariable<Quaternion>();
        protected virtual void Initiate()
        {
            OnUpdateAction += Movement;
        }
        private void Update()
        {
            if (!IsSpawned)
            {
                return;
            }
            OnUpdate();
        }
        private void OnUpdate()
        {
            OnUpdateAction?.Invoke();
        }

        [ServerRpc]
        protected void UpdatePositionServerRpc(Vector3 position)
        {
            serverPosition.Value = position;
        }

        [ServerRpc]
        protected void UpdateRotationServerRpc(Quaternion rotation)
        {
            serverRotation.Value = rotation;
        }

        public abstract void Movement();
    }
}