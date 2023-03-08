using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace Lesson4
{
    public class NetworkHUD : MonoBehaviour
    {
        [SerializeField]
        private Button _startServerButton;
        [SerializeField]
        private Button _startClientButton;
        [SerializeField]
        private Button _startHostButton;
        [SerializeField]
        private Button _shutdownNetworkButton;

        private void Start()
        {
            _startClientButton.onClick.AddListener(() => StartClientButton());
            _startServerButton.onClick.AddListener(() => StartServerButton());
            _startHostButton.onClick.AddListener(() => StartHostButton());
            _shutdownNetworkButton.onClick.AddListener(() => ShutdownNetworkButton());
        }

        public void StartClientButton()
        {
            NetworkManager.Singleton.StartClient();
        }

        public void StartServerButton()
        {
            NetworkManager.Singleton.StartServer();
        }

        public void StartHostButton()
        {
            NetworkManager.Singleton.StartHost();
        }
        
        public void ShutdownNetworkButton()
        {
            NetworkManager.Singleton.Shutdown();
        }
    }
}