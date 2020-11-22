using System;
using System.Collections.Generic;
using System.Text;

namespace OscDotNet.Lib
{
    public class MalformedMessageException : InvalidOperationException
    {
        private byte[] messageData;

        public byte[] MessageData { get { return messageData; } }

        public MalformedMessageException(string message, byte[] data)
            : base(message) 
        {
            if (data != null) {
                messageData = new byte[data.Length];
                Array.Copy(data, messageData, data.Length);
            }
        }

        public MalformedMessageException(string message, byte[] data, Exception innerException)
            : base(message, innerException) 
        {
            if (data != null) {
                messageData = new byte[data.Length];
                Array.Copy(data, messageData, data.Length);
            }
        }
    }
}
