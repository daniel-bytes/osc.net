using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace osc.net.unittests.Message
{
    [TestClass]
    public class MessageParserTests
    {
        [TestMethod]
        public void MessageParser_Test_Parse()
        {
            MessageParser parser = new MessageParser();
            MessageBuilder builder = new MessageBuilder();
            builder.SetAddress("/test");
            builder.PushAtom(new Atom(TypeTag.OscInt32));
            builder.PushAtom(new byte[] { (byte)5 });
            builder.PushAtom(5.0f);
            builder.PushAtom(6.0);
            builder.PushAtom(7);
            builder.PushAtom(8L);
            builder.PushAtom("Test");


            // Test
            osc.net.Message message = builder.ToMessage();
            
            byte[] bytes = parser.Parse(message);
            osc.net.Message parsedMessage = parser.Parse(bytes);
            byte[] reparsedBytes = parser.Parse(parsedMessage);

            Assert.AreEqual(message, parsedMessage);
            CollectionAssert.AreEqual(bytes, reparsedBytes);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MessageParser_Test_SerializeAddress_Fails_InvalidAddress()
        {
            MessageParser parser = new MessageParser();
            byte[] bytes = GetTestBytes();

            Assert.AreEqual(60, bytes.Length);

            bytes[0] = (byte)'\\';

            parser.Parse(bytes);
        }

        [TestMethod]
        [ExpectedException(typeof(MalformedMessageException))]
        public void MessageParser_Test_SerializeAddress_Fails_InvalidAddressPadding()
        {
            MessageParser parser = new MessageParser();
            byte[] bytes = GetTestBytes();

            Assert.AreEqual(60, bytes.Length);

            bytes[1] = byte.MinValue;

            parser.Parse(bytes);
        }

        [TestMethod]
        [ExpectedException(typeof(MalformedMessageException))]
        public void MessageParser_Test_SerializeAddress_Fails_InvalidAddressDelimiter()
        {
            MessageParser parser = new MessageParser();
            byte[] bytes = GetTestBytes();

            Assert.AreEqual(60, bytes.Length);

            bytes[1] = (byte)',';

            parser.Parse(bytes);
        }

        [TestMethod]
        [ExpectedException(typeof(MalformedMessageException))]
        public void MessageParser_Test_SerializeTypeTags_Fails_NullTypeTag()
        {
            MessageParser parser = new MessageParser();
            byte[] bytes = GetTestBytes();

            Assert.AreEqual(60, bytes.Length);

            bytes[9] = byte.MinValue;

            parser.Parse(bytes);
        }

        [TestMethod]
        [ExpectedException(typeof(MalformedMessageException))]
        public void MessageParser_Test_SerializeTypeTags_Fails_InvalidTypeTag()
        {
            MessageParser parser = new MessageParser();
            byte[] bytes = GetTestBytes();

            Assert.AreEqual(60, bytes.Length);

            bytes[9] = (byte)'x';

            parser.Parse(bytes);
        }

        private byte[] GetTestBytes()
        {
            MessageParser parser = new MessageParser();
            MessageBuilder builder = new MessageBuilder();
            builder.SetAddress("/test");
            builder.PushAtom(new Atom(TypeTag.OscInt32));
            builder.PushAtom(new byte[] { (byte)5 });
            builder.PushAtom(5.0f);
            builder.PushAtom(6.0);
            builder.PushAtom(7);
            builder.PushAtom(8L);
            builder.PushAtom("Test");

            return parser.Parse(builder.ToMessage());
        }
    }
}
