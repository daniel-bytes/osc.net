using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

namespace OscDotNet.Lib
{
    public delegate void OnMessageReceivedEventHandler(object sender, MessageReceivedEventArgs e);

    public interface IOscServer : IDisposable
    {
        OscEndpoint Endpoint { get; }
        event OnMessageReceivedEventHandler MessageReceived;

        void BeginListen();
        void EndListen();
    }

    public class MessageReceivedEventArgs : EventArgs
    {
        public Message Message { get; private set; }

        public MessageReceivedEventArgs(Message message) {
            this.Message = message;
        }
    }

    public class OscUdpServer : IOscServer
    {
        private static MessageParser defaultMessageParser = new MessageParser();
        private Socket socket;
        private bool islistening;

        public OscEndpoint Endpoint { get; private set; }
        public event OnMessageReceivedEventHandler MessageReceived;

        public OscUdpServer(OscEndpoint endpoint) {
            this.Endpoint = endpoint;
            this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            socket.Bind(
                this.Endpoint.CreateIpEndpoint()
                );
        }

        public void BeginListen() {
            if (!islistening) {
                islistening = true;
                OnListen();
            }
        }

        public void EndListen() {
            if (islistening) {
                islistening = false;
            }
        }

        protected virtual void OnMessageReceived(MessageReceivedEventArgs args) {
            var temp = MessageReceived;

            if (temp != null) {
                temp(this, args);
            }
        }


        private void OnListen() {
            if (!islistening) return;

            var buffer = new byte[8096];
            socket.BeginReceive(
                buffer,
                0,
                buffer.Length,
                SocketFlags.None,
                (ia) => {
                    int bytesReceived = socket.EndReceive(ia);

                    try {
                        if (bytesReceived > 0) {
                            var byteBuffer = (byte[])ia.AsyncState;
                            Message msg = defaultMessageParser.Parse(byteBuffer);

                            OnMessageReceived(
                                new MessageReceivedEventArgs(msg)
                                );
                        }

                        OnListen();
                    }
                    catch (MalformedMessageException) {
                        OnListen();
                        throw;
                    }
                    catch {
                        islistening = false;
                        throw;
                    }
                },
                buffer);
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {
            if (disposing) {
                EndListen();
            }
        }
    }
}
