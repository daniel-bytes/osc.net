using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace OscDotNet.Lib
{
    public delegate void OnClientConnectedCallback();
    public delegate void OnClientDisconnectedCallback();
    public delegate void OnMessageSentCallback(bool messageSent);

    public interface IOscClient : IDisposable
    {
        OscEndpoint Endpoint { get; }
        
        void Connect();
        void ConnectAsync(OnClientConnectedCallback clientConnected);
        
        void Disconnect();
        void DisconnectAsync(OnClientDisconnectedCallback clientDisconnected);

        bool SendMessage(Message message);
        void SendMessageAsync(Message message, OnMessageSentCallback messageSent);
    }

    public class OscUdpClient : IOscClient
    {
        private static MessageParser defaultMessageParser = new MessageParser();
        private Socket socket;

        public OscEndpoint Endpoint { get; private set; }

        public OscUdpClient(OscEndpoint endpoint)
            : this(endpoint, false)
        {

        }

        public OscUdpClient(OscEndpoint endpoint, bool connect) {
            this.Endpoint = endpoint;
            this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            if (connect) {
                Connect();
            }
        }

        public void Connect() {
            if (!socket.Connected) {
                socket.Connect(
                    this.Endpoint.CreateIpEndpoint()
                    );
            }
        }

        public void ConnectAsync(OnClientConnectedCallback callback) {
            if (socket.Connected) {
                if (callback != null) {
                    callback();
                }
            }
            else {
                socket.BeginConnect(
                    this.Endpoint.CreateIpEndpoint(),
                    (ia) => {
                        socket.EndConnect(ia);

                        if (callback != null) {
                            callback();
                        }
                    },
                    null);
            }
        }

        public void Disconnect() {
            if (socket.Connected) {
                socket.Disconnect(true);
            }
        }

        public void DisconnectAsync(OnClientDisconnectedCallback callback) {
            if (socket.Connected) {
                socket.BeginDisconnect(
                    true,
                    (ia) => {
                        socket.EndDisconnect(ia);

                        if (callback != null) {
                            callback();
                        }
                    }, 
                    null);
            }
            else {
                if (callback != null) {
                    callback();
                }
            }
        }

        public bool SendMessage(Message message) {
            var bytes = defaultMessageParser.Parse(message);
            return ( socket.Send(bytes) == bytes.Length );
        }

        public void SendMessageAsync(Message message, OnMessageSentCallback callback) {
            var bytes = defaultMessageParser.Parse(message);

            socket.BeginSend(
                bytes, 
                0, 
                bytes.Length, 
                SocketFlags.None,
                (ia) => { 
                    int bytesSent = socket.EndSend(ia);

                    if (callback != null) {
                        callback(bytesSent == bytes.Length);
                    }
                },
                null);
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {
            if (disposing) {
                Disconnect();
            }
        }
    }
}
