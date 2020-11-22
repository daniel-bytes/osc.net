using System;
using OscDotNet.Lib;
using Xunit;

namespace OscDotNet.Tests
{
    public class MessageParserTests
    {
        [Fact]
        public void MessageParser_Test_Parse()
        {
            var parser = new MessageParser();
            var builder = new MessageBuilder();
            builder.SetAddress("/test");
            builder.PushAtom(new Atom(TypeTag.OscInt32));
            builder.PushAtom(new byte[] { (byte)5 });
            builder.PushAtom(5.0f);
            builder.PushAtom(6.0);
            builder.PushAtom(7);
            builder.PushAtom(8L);
            builder.PushAtom("Test");


            // Test
            Message message = builder.ToMessage();
            
            byte[] bytes = parser.Parse(message);
            Message parsedMessage = parser.Parse(bytes);
            byte[] reparsedBytes = parser.Parse(parsedMessage);

            Assert.Equal(message, parsedMessage);
            Assert.Equal(bytes, reparsedBytes);
        }

        [Fact]
        public void MessageParser_Test_SerializeAddress_Fails_InvalidAddress()
        {
            var parser = new MessageParser();
            byte[] bytes = GetTestBytes();

            Assert.Equal(60, bytes.Length);

            bytes[0] = (byte)'\\';

            Assert.Throws<ArgumentException>(() => parser.Parse(bytes));
        }

        [Fact]
        public void MessageParser_Test_SerializeAddress_Fails_InvalidAddressPadding()
        {
            var parser = new MessageParser();
            byte[] bytes = GetTestBytes();

            Assert.Equal(60, bytes.Length);

            bytes[1] = byte.MinValue;

            Assert.Throws<MalformedMessageException>(() => parser.Parse(bytes));
        }

        [Fact]
        public void MessageParser_Test_SerializeAddress_Fails_InvalidAddressDelimiter()
        {
            var parser = new MessageParser();
            byte[] bytes = GetTestBytes();

            Assert.Equal(60, bytes.Length);

            bytes[1] = (byte)',';

            Assert.Throws<MalformedMessageException>(() => parser.Parse(bytes));
        }

        [Fact]
        public void MessageParser_Test_SerializeTypeTags_Fails_NullTypeTag()
        {
            var parser = new MessageParser();
            byte[] bytes = GetTestBytes();

            Assert.Equal(60, bytes.Length);

            bytes[9] = byte.MinValue;

            Assert.Throws<MalformedMessageException>(() => parser.Parse(bytes));
        }

        [Fact]
        public void MessageParser_Test_SerializeTypeTags_Fails_InvalidTypeTag()
        {
            var parser = new MessageParser();
            byte[] bytes = GetTestBytes();

            Assert.Equal(60, bytes.Length);

            bytes[9] = (byte)'x';

            Assert.Throws<MalformedMessageException>(() => parser.Parse(bytes));
        }

        private byte[] GetTestBytes()
        {
            var parser = new MessageParser();
            var builder = new MessageBuilder();
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
