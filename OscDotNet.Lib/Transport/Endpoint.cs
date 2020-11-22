using System;
using System.Collections.Generic;
using System.Net;

namespace OscDotNet.Lib
{
    public class OscEndpoint
    {
        public string Address { get; set; }
        public int Port { get; set; }

        public OscEndpoint()
            : this(10000)
        {
        }

        public OscEndpoint(int port) 
            : this("127.0.0.1", port)
        {
        }

        public OscEndpoint(string address, int port) {
            this.Address = address;
            this.Port = port;
        }

        public OscEndpoint(IPEndPoint endpoint) {
            this.Address = endpoint.Address.ToString();
            this.Port = endpoint.Port;
        }

        public IPEndPoint CreateIpEndpoint() {
            var addr = IPAddress.Parse(Address);
            return new IPEndPoint(addr, Port);
        }
    }
}
