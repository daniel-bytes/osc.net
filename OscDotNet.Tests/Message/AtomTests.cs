using System;
using System.Text;
using OscDotNet.Lib;
using Xunit;

namespace OscDotNet.Tests
{
    public class AtomTests
    {
        [Fact]
        public void Atom_Test_Int()
        {
            Atom atom1 = new Atom(5);
            Atom atom2 = 5;
            Atom atom3 = (6);
            Atom atom4 = (5.0f);

            Assert.Equal(atom1, atom2);
            Assert.NotEqual(atom1, atom3);
            Assert.NotEqual(atom1, atom4);
            Assert.Equal(TypeTag.OscInt32, atom1.TypeTag);
            Assert.Equal(5, atom1.Int32Value);
            Assert.Equal(5, (int)atom1);
        }

        [Fact]
        public void Atom_Test_Int_InvalidCast()
        {
            Atom atom1 = new Atom(5.0);
            Assert.Throws<InvalidCastException>(() => (int)atom1);
        }

        [Fact]
        public void Atom_Test_Float()
        {
            Atom atom1 = new Atom(5.0f);
            Atom atom2 = 5.0f;
            Atom atom3 = (6.0f);
            Atom atom4 = (5);
            Atom atom5 = 5.0; // double

            Assert.Equal(atom1, atom2);
            Assert.NotEqual(atom1, atom3);
            Assert.NotEqual(atom1, atom4);
            Assert.Equal(atom1, atom5);
            Assert.Equal(TypeTag.OscFloat32, atom1.TypeTag);
            Assert.Equal(TypeTag.OscFloat32, atom5.TypeTag);
            Assert.Equal(5.0f, atom1.Float32Value);
            Assert.Equal(5.0f, (float)atom1);
        }

        [Fact]
        public void Atom_Test_Float_InvalidCast()
        {
            Atom atom1 = new Atom(5);
            Assert.Throws<InvalidCastException>(() => (float)atom1);
        }

        [Fact]
        public void Atom_Test_String()
        {
            Atom atom1 = new Atom("This is a test");
            Atom atom2 = "This is a test";
            Atom atom3 = "This is another test";
            Atom atom4 = Encoding.ASCII.GetBytes("This is a test");

            Assert.Equal(atom1, atom2);
            Assert.NotEqual(atom1, atom3);
            Assert.NotEqual(atom1, atom4);
            Assert.Equal("This is a test", atom1);
            Assert.Equal(TypeTag.OscString, atom1.TypeTag);
            Assert.Equal("This is a test", (string)atom1);
        }

        [Fact]
        public void Atom_Test_String_InvalidCast()
        {
            Atom atom1 = new Atom(5);
            Assert.Throws<InvalidCastException>(() => (string)atom1);
        }

        [Fact]
        public void Atom_Test_Blob()
        {
            var byteSeq1 = Encoding.ASCII.GetBytes("ABC");
            var byteSeq2 = Encoding.ASCII.GetBytes("XYZ");

            Atom atom1 = new Atom(byteSeq1);
            Atom atom2 = byteSeq1;
            Atom atom3 = byteSeq2;
            Atom atom4 = Encoding.ASCII.GetString(byteSeq1);

            Assert.Equal(atom1, atom2);
            Assert.NotEqual(atom1, atom3);
            Assert.NotEqual(atom1, atom4);
            Assert.Equal(byteSeq1, atom1);
            Assert.Equal(TypeTag.OscBlob, atom1.TypeTag);
            Assert.Equal(byteSeq1, (byte[])atom1);
        }

        [Fact]
        public void Atom_Test_Blob_InvalidCast()
        {
            Atom atom1 = new Atom("Not a blob value");
            Assert.Throws<InvalidCastException>(() => (byte[])atom1);
        }
    }
}
