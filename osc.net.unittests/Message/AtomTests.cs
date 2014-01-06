using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace osc.net.unittests.Message
{
    [TestClass]
    public class AtomTests
    {
        [TestMethod]
        public void Atom_Test_Int()
        {
            Atom atom1 = new Atom(5);
            Atom atom2 = 5;
            Atom atom3 = (6);
            Atom atom4 = (5.0f);

            Assert.IsFalse(ReferenceEquals(atom1, atom2));
            Assert.AreEqual(atom1, atom2);
            Assert.AreNotEqual(atom1, atom3);
            Assert.AreNotEqual(atom1, atom4);
            Assert.AreEqual(TypeTag.OscInt32, atom1.TypeTag);
            Assert.AreEqual(5, atom1.Int32Value);
            Assert.AreEqual(5, (int)atom1);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void Atom_Test_Int_InvalidCast()
        {
            Atom atom1 = new Atom(5.0);
            int ivalue = (int)atom1;
        }

        [TestMethod]
        public void Atom_Test_Float()
        {
            Atom atom1 = new Atom(5.0f);
            Atom atom2 = 5.0f;
            Atom atom3 = (6.0f);
            Atom atom4 = (5);
            Atom atom5 = 5.0; // double

            Assert.IsFalse(ReferenceEquals(atom1, atom2));
            Assert.AreEqual(atom1, atom2);
            Assert.AreNotEqual(atom1, atom3);
            Assert.AreNotEqual(atom1, atom4);
            Assert.AreEqual(atom1, atom5);
            Assert.AreEqual(TypeTag.OscFloat32, atom1.TypeTag);
            Assert.AreEqual(TypeTag.OscFloat32, atom5.TypeTag);
            Assert.AreEqual(5.0f, atom1.Float32Value);
            Assert.AreEqual(5.0f, (float)atom1);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void Atom_Test_Float_InvalidCast()
        {
            Atom atom1 = new Atom(5);
            float ivalue = (float)atom1;
        }

        [TestMethod]
        public void Atom_Test_String()
        {
            Atom atom1 = new Atom("This is a test");
            Atom atom2 = "This is a test";
            Atom atom3 = "This is another test";
            Atom atom4 = Encoding.ASCII.GetBytes("This is a test");

            Assert.IsFalse(ReferenceEquals(atom1, atom2));
            Assert.AreEqual(atom1, atom2);
            Assert.AreNotEqual(atom1, atom3);
            Assert.AreNotEqual(atom1, atom4);
            Assert.AreEqual("This is a test", atom1);
            Assert.AreEqual(TypeTag.OscString, atom1.TypeTag);
            Assert.AreEqual("This is a test", (string)atom1);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void Atom_Test_String_InvalidCast()
        {
            Atom atom1 = new Atom(5);
            string ivalue = (string)atom1;
        }

        [TestMethod]
        public void Atom_Test_Blob()
        {
            var byteSeq1 = Encoding.ASCII.GetBytes("ABC");
            var byteSeq2 = Encoding.ASCII.GetBytes("XYZ");

            Atom atom1 = new Atom(byteSeq1);
            Atom atom2 = byteSeq1;
            Atom atom3 = byteSeq2;
            Atom atom4 = Encoding.ASCII.GetString(byteSeq1);

            Assert.IsFalse(ReferenceEquals(atom1, atom2));
            Assert.AreEqual(atom1, atom2);
            Assert.AreNotEqual(atom1, atom3);
            Assert.AreNotEqual(atom1, atom4);
            Assert.AreEqual(byteSeq1, atom1);
            Assert.AreEqual(TypeTag.OscBlob, atom1.TypeTag);
            Assert.AreEqual(byteSeq1, (byte[])atom1);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void Atom_Test_Blob_InvalidCast()
        {
            Atom atom1 = new Atom("Not a blob value");
            byte[] ivalue = (byte[])atom1;
        }
    }
}
