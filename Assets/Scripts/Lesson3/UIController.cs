using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Lesson3
{
    public class UIController : MonoBehaviour
    {
        [SerializeField]
        private Button _buttonStartServer;
        [SerializeField]
        private Button _buttonShutDownServer;
        [SerializeField]
        private Button _buttonConnectClient;
        [SerializeField]
        private Button _buttonDisconnectClient;
        [SerializeField]
        private Button _buttonSendMessage;
        [SerializeField]
        private Button _changeName;

        [SerializeField]
        private TMP_InputField _inputField;
        [SerializeField]
        private TextField _textField;
        [SerializeField]
        private TMP_InputField _nameInputField;
        [SerializeField]
        private Server _server;
        [SerializeField]
        private Client _client;

        private void Start()
        {
            _buttonStartServer.onClick.AddListener(() => StartServer());
            _buttonShutDownServer.onClick.AddListener(() => ShutDownServer());
            _buttonConnectClient.onClick.AddListener(() => Connect());
            _buttonDisconnectClient.onClick.AddListener(() => Disconnect());
            _buttonSendMessage.onClick.AddListener(() => SendMessage());
            _changeName.onClick.AddListener(() => ChangeName());
            _client.onMessageReceive += ReceiveMessage;
        }

        private void StartServer()
        {
            _server.StartServer();
        }

        private void ShutDownServer()
        {
            _server.ShutDownServer();
        }

        private void Connect()
        {
            _client.Connect();
        }

        private void Disconnect()
        {
            _client.Disconnect();
        }

        private void SendMessage()
        {
            _client.SendMessag(_inputField.text);
            _inputField.text = "";
        }

        private void ChangeName()
        {
            _client.ChangeName(_nameInputField.text);
            _inputField.text = "";
        }

        public void ReceiveMessage(object message)
        {
            _textField.ReceiveMessage(message);
        }
    }
}