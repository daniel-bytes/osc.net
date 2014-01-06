using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace osc.net.unittests.Message
{
    [TestClass]
    public class MessageBuilderTests
    {
        [TestMethod]
        public void MessageBuilder_Test_SetAddress_Succeeds()
        {
            var builder = new MessageBuilder();
            Assert.AreEqual("/", builder.GetAddress());

            builder.SetAddress("/this/is/a/test");
            Assert.AreEqual("/this/is/a/test", builder.GetAddress());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MessageBuilder_Test_SetAddress_Fails_AddressIsNull()
        {
            var builder = new MessageBuilder();
            builder.SetAddress(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MessageBuilder_Test_SetAddress_Fails_AddressIsEmpty()
        {
            var builder = new MessageBuilder();
            builder.SetAddress("");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MessageBuilder_Test_SetAddress_Fails_AddressIsInvalid()
        {
            var builder = new MessageBuilder();
            builder.SetAddress("this/is/a/test");
        }

        [TestMethod]
        public void MessageBuilder_Test_PushPopSetAtoms()
        {
            var builder = new MessageBuilder();

            builder.PushAtom(new Atom(TypeTag.OscInt32));
            builder.PushAtom(new byte[] { (byte)5 });
            builder.PushAtom(5.0f);
            builder.PushAtom(6.0);
            builder.PushAtom(7);
            builder.PushAtom(8L);
            builder.PushAtom("Test");

            Assert.AreEqual(7, builder.AtomCount);

            var last = builder.PopAtom();
            Assert.AreEqual(6, builder.AtomCount);
            Assert.AreEqual("Test", last);

            builder.SetAtom(5, 1234);
            Assert.AreEqual(1234, builder.GetAtom(5));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void MessageBuilder_Test_PopAtom_Fails_CollectionEmpty()
        {
            var builder = new MessageBuilder();
            builder.PopAtom();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void MessageBuilder_Test_SetAtom_Fails_InvalidIndex()
        {
            var builder = new MessageBuilder();
            builder.SetAtom(0, "test");
        }

        [TestMethod]
        public void MessageBuilder_Test_Reset()
        {
            var builder = new MessageBuilder();
            builder.SetAddress("/test");
            builder.PushAtom(1234);

            Assert.AreEqual("/test", builder.GetAddress());
            Assert.AreEqual(1, builder.AtomCount);

            builder.Reset();
            Assert.AreEqual("/", builder.GetAddress());
            Assert.AreEqual(0, builder.AtomCount);
        }

        [TestMethod]
        public void MessageBuilder_Test_ToMessage()
        {
            var builder = new MessageBuilder();
            builder.SetAddress("/test");
            builder.PushAtom(new Atom(TypeTag.OscInt32));
            builder.PushAtom(new byte[] { (byte)5 });
            builder.PushAtom(5.0f);
            builder.PushAtom(6.0);
            builder.PushAtom(7);
            builder.PushAtom(8L);
            builder.PushAtom("Test");

            var message = builder.ToMessage();

            Assert.AreEqual("/test", message.Address);
            Assert.AreEqual(7, message.Atoms.Length);
            Assert.AreEqual(TypeTag.OscInt32, message.Atoms[0].TypeTag);
            Assert.AreEqual(TypeTag.OscBlob, message.Atoms[1].TypeTag);
            Assert.AreEqual(TypeTag.OscFloat32, message.Atoms[2].TypeTag);
            Assert.AreEqual(TypeTag.OscFloat32, message.Atoms[3].TypeTag);
            Assert.AreEqual(TypeTag.OscInt32, message.Atoms[4].TypeTag);
            Assert.AreEqual(TypeTag.OscInt32, message.Atoms[5].TypeTag);
            Assert.AreEqual(TypeTag.OscString, message.Atoms[6].TypeTag);
        }
    }
}
