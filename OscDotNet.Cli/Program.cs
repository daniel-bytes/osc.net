using System;
using System.Text;
using OscDotNet.Lib;

namespace OscDotNet.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            bool isServer = false;
            int port = 10000;

            if (args.Length > 0)
            {
                isServer = args[0].Equals("server");
            }
            else
            {
                Console.WriteLine("Is this a server?");
                var response = Console.ReadLine().Trim().ToLower();
                isServer = response.Equals("yes") || response.Equals("y");
            }

            string strport;
            if (args.Length > 1)
            {
                strport = args[1];
            }
            else
            {
                Console.WriteLine("What port?");
                strport = Console.ReadLine().Trim();

            }

            if (int.TryParse(strport, out int temp)) port = temp;

            if (isServer)
            {
                var server = new OscUdpServer(
                    new OscEndpoint(port)
                    );

                server.MessageReceived += (s, e) => {
                    Console.WriteLine("Message received:");
                    PrintMessage(e.Message);
                };

                Console.WriteLine("Begin listening on port {0}", port);
                server.BeginListen();

                Console.WriteLine("Press 'q' to quit...");
                var value = "";

                while (value.ToLower() != "q")
                {
                    value = Console.ReadLine();
                }
            }
            else
            {
                var client = new OscUdpClient(
                    new OscEndpoint(port)
                    );

                Console.WriteLine("Begin sending messages on port {0}", port);
                client.Connect();

                Console.WriteLine("Enter message data, or type 'q' to quit.\r\nEx: /foo/bar iii 1 2 3");

                var value = Console.ReadLine().Trim();

                while (value.ToLower() != "q")
                {
                    var parts = value.Split(new string[] { " " }, StringSplitOptions.None);
                    var builder = new MessageBuilder();

                    try
                    {
                        builder.SetAddress(parts[0]);
                        for (int i = 0; i < parts[1].Length; i++)
                        {
                            switch (parts[1][i])
                            {
                                case 'i':
                                    builder.PushAtom(int.Parse(parts[2 + i]));
                                    break;

                                case 'f':
                                    builder.PushAtom(float.Parse(parts[2 + i]));
                                    break;

                                case 's':
                                    builder.PushAtom(parts[2 + i]);
                                    break;

                                case 'b':
                                    builder.PushAtom(Encoding.ASCII.GetBytes(parts[2 + i]));
                                    break;
                            }
                        }

                        Console.WriteLine("Sending message...");
                        try
                        {
                            client.SendMessage(builder.ToMessage());
                        }
                        catch (Exception exc)
                        {
                            Console.WriteLine("Failed to send message: {0}", exc.Message);
                        }
                        Console.WriteLine("Message Sent!");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Failed to build message: {0}", ex.Message);
                    }

                    value = Console.ReadLine().Trim();
                }
            }
        }

        static void TestMessageToBytes()
        {
            var messageBuilder = new MessageBuilder();
            messageBuilder.SetAddress("/a/b/c");
            messageBuilder.PushAtom(123);
            messageBuilder.PushAtom(321.123f);
            messageBuilder.PushAtom("foObar");
            messageBuilder.PushAtom(Encoding.ASCII.GetBytes("foObarR"));

            var message = messageBuilder.ToMessage();
            PrintMessage(message);

            var parser = new MessageParser();
            var bytes = parser.Parse(message);

            // round trip
            var newmessage = parser.Parse(bytes);
            System.Diagnostics.Debug.Assert(message.Equals(newmessage), "Roundtrip messages are not equal.");
            PrintMessage(newmessage);
        }

        static void TestBytesToMessage()
        {
            byte[] b = new byte[1000];
            byte[] ival = BitConverter.GetBytes(123);
            byte[] fval = BitConverter.GetBytes(321.123f);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(ival);
                Array.Reverse(fval);
            }

            int i = 0;
            b[i++] = (byte)'/';
            b[i++] = (byte)'a';
            b[i++] = (byte)'/';
            b[i++] = (byte)'b';
            b[i++] = (byte)'/';         // 4
            b[i++] = (byte)'c';
            b[i++] = (byte)0;
            b[i++] = (byte)0;
            b[i++] = (byte)',';         // 8
            b[i++] = (byte)'i';
            b[i++] = (byte)'f';
            b[i++] = (byte)'s';
            b[i++] = (byte)'b';         // 12
            b[i++] = byte.MinValue;
            b[i++] = byte.MinValue;
            b[i++] = byte.MinValue;
            b[i++] = ival[0];           // 16
            b[i++] = ival[1];
            b[i++] = ival[2];
            b[i++] = ival[3];
            b[i++] = fval[0];           // 20
            b[i++] = fval[1];
            b[i++] = fval[2];
            b[i++] = fval[3];
            b[i++] = 0;                 // 24
            b[i++] = 0;
            b[i++] = 0;
            b[i++] = 6;
            b[i++] = (byte)'f';         // 28
            b[i++] = (byte)'o';
            b[i++] = (byte)'O';
            b[i++] = (byte)'b';
            b[i++] = (byte)'a';         // 32
            b[i++] = (byte)'r';
            b[i++] = byte.MinValue;
            b[i++] = byte.MinValue;
            b[i++] = 0;                 // 36
            b[i++] = 0;
            b[i++] = 0;
            b[i++] = 7;
            b[i++] = (byte)'f';         // 40
            b[i++] = (byte)'o';
            b[i++] = (byte)'O';
            b[i++] = (byte)'b';
            b[i++] = (byte)'a';         // 44
            b[i++] = (byte)'r';
            b[i++] = (byte)'R';
            b[i++] = byte.MinValue;

            var parser = new MessageParser();
            var message = parser.Parse(b);
            PrintMessage(message);
        }

        static void PrintMessage(Message message)
        {
            Console.WriteLine("Message:");
            Console.WriteLine("- Address: '{0}'", message.Address);
            Console.WriteLine("- TypeTags: '{0}'", GetTypeTagsString(message.TypeTags));
            Console.WriteLine("- Atoms:");

            foreach (var atom in message)
            {
                Console.WriteLine("  => {0}", atom.ToString());
            }
        }

        static string GetTypeTagsString(TypeTag[] tags)
        {
            var builder = new StringBuilder();

            foreach (var tag in tags)
            {
                byte b = (byte)tag;
                char c = (char)b;
                builder.Append(c);
            }

            return builder.ToString();
        }
    }
}
