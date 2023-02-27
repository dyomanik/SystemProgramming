using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Lesson3
{
    public class Client : MonoBehaviour
    {
        public delegate void OnMessageReceive(object message);
        public event OnMessageReceive onMessageReceive;
        private const int MAX_CONNECTION = 10;
        private int _port = 0;
        private int _serverPort = 5805;
        private int _hostID;
        private int _reliableChannel;
        private int _connectionID;
        private bool _isConnected = false;
        private byte _error;
        private string _clientName;

        public void Connect()
        {
            if (_isConnected)
            {
                return;
            }
            NetworkTransport.Init();
            ConnectionConfig cc = new ConnectionConfig();
            _reliableChannel = cc.AddChannel(QosType.Reliable);
            HostTopology topology = new HostTopology(cc, MAX_CONNECTION);
            _hostID = NetworkTransport.AddHost(topology, _port);
            _connectionID = NetworkTransport.Connect(_hostID, "127.0.0.1", _serverPort, 0, out _error);
            if ((NetworkError)_error == NetworkError.Ok)
            {
                _isConnected = true;
            }
            else
            {
                Debug.Log((NetworkError)_error);
            }
        }

        public void Disconnect()
        {
            if (!_isConnected)
            {
                return;
            }
            NetworkTransport.Disconnect(_hostID, _connectionID, out _error);
            //_isConnected = false;
        }

        void Update()
        {
            if (!_isConnected)
            {
                return;
            }
            int recHostId;
            int connectionId;
            int channelId;
            byte[] recBuffer = new byte[1024];
            int bufferSize = 1024;
            int dataSize;
            NetworkEventType recData = NetworkTransport.Receive(out recHostId, out connectionId, out
            channelId, recBuffer, bufferSize, out dataSize, out _error);

            while (recData != NetworkEventType.Nothing)
            {
                switch (recData)
                {
                    case NetworkEventType.Nothing:
                        break;

                    case NetworkEventType.ConnectEvent:
                        onMessageReceive?.Invoke($"You have been connected to server.");
                        if (_clientName != null)
                        {
                            SendMessag(_clientName);
                        }
                        Debug.Log($"You have been connected to server.");
                        break;

                    case NetworkEventType.DataEvent:
                        string message = Encoding.Unicode.GetString(recBuffer, 0, dataSize);
                        onMessageReceive?.Invoke(message);
                        Debug.Log(message);
                        break;

                    case NetworkEventType.DisconnectEvent:
                        _isConnected = false;
                        onMessageReceive?.Invoke($"You have been disconnected from server.");
                        Debug.Log($"You have been disconnected from server.");
                        break;

                    case NetworkEventType.BroadcastEvent:
                        break;
                }
                recData = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer,
                bufferSize, out dataSize, out _error);
            }
        }

        public void SendMessag(string message)
        {
            byte[] buffer = Encoding.Unicode.GetBytes(message);
            NetworkTransport.Send(_hostID, _connectionID, _reliableChannel, buffer, message.Length *
            sizeof(char), out _error);
            if ((NetworkError)_error != NetworkError.Ok)
            {
                Debug.Log((NetworkError)_error);
            }
        }

        public void ChangeName(string name)
        {
            if (!_isConnected && name != string.Empty)
            {
                _clientName = name;
            }
            else if (_isConnected)
            {
                onMessageReceive?.Invoke($"Change your name below");
            }
            else
            {
                onMessageReceive?.Invoke($"First you need enter your name");
            }
        }
    }
}