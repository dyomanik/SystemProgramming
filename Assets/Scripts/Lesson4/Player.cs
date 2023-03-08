using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Lesson4
{
    public class Player : NetworkBehaviour
    {
        [SerializeField]
        private GameObject _playerPrefab;
        [SerializeField]
        private List<GameObject> _spawnTransforms;
        private GameObject _playerCharacter;
        private void Start()
        {
            SpawnCharacter();
        }
        private void SpawnCharacter()
        {
            if (!IsServer)
            {
                return;
            }
            _playerCharacter = Instantiate(_playerPrefab);
            _playerCharacter.GetComponent<NetworkObject>().SpawnAsPlayerObject(NetworkObject.OwnerClientId);
        }
    }
}