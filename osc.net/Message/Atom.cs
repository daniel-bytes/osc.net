using System;
using System.Runtime.InteropServices;

namespace osc.net
{
    // Note that we can only overlap pure value types to get a C-style "union" type.
    // The string/byte[]/"ref type" objects must live the objvalue field at offset 8.
    [StructLayout(LayoutKind.Explicit)]
    public struct Atom
    {
        [FieldOffset(0)]
        private TypeTag typetag;
        
        [FieldOffset(4)]
        private int int32value;

        [FieldOffset(4)]
        private float float32value;

        [FieldOffset(8)]
        private object objvalue;// Properties

        public TypeTag TypeTag {
            get { return typetag; }
            set {
                typetag = value;
            }
        }

        public int Int32Value {
            get { return int32value; }
            set {
                int32value = value;
                TypeTag = TypeTag.OscInt32;
            }
        }

        public float Float32Value {
            get { return float32value; }
            set {
                float32value = value;
                TypeTag = TypeTag.OscFloat32;
            }
        }

        public string StringValue {
            get { return objvalue as string; }
            set {
                objvalue = value;
                TypeTag = TypeTag.OscString;
            }
        }

        public byte[] BlobValue {
            get { return objvalue as byte[]; }
            set {
                objvalue = value;
                TypeTag = TypeTag.OscBlob;
            }
        }

        public Atom(TypeTag type) {
            typetag = type;
            int32value = 0;
            float32value = 0;
            objvalue = null;
        }

        public Atom(int value) {
            float32value = 0;
            objvalue = null;
            int32value = value;
            typetag = TypeTag.OscInt32;
        }

        public Atom(float value) {
            int32value = 0;
            objvalue = null;
            float32value = value;
            typetag = TypeTag.OscFloat32;
        }

        public Atom(string value) {
            int32value = 0;
            float32value = 0;
            objvalue = value;
            typetag = TypeTag.OscString;
        }

        public Atom(byte[] value) {
            int32value = 0;
            float32value = 0;
            objvalue = value;
            typetag = TypeTag.OscBlob;
        }


        // Methods
        public override string ToString() {
            switch (typetag) {
                case net.TypeTag.OscInt32:
                    return Int32Value.ToString();
                case net.TypeTag.OscFloat32:
                    return Float32Value.ToString();
                case net.TypeTag.OscString:
                    return StringValue ?? "null";
                case net.TypeTag.OscBlob:
                    return ( BlobValue != null ? BitConverter.ToString(BlobValue) : "null" );
                default:
                    return "unknown";
            }
        }

        public override bool Equals(object obj) {
            if (!( obj is Atom )) return false;
            return Equals((Atom)obj);
        }

        public bool Equals(Atom rhs) {
            return AtomEqualityComparer.DefaultInstance.Equals(this, rhs);
        }

        public override int GetHashCode() {
            return AtomEqualityComparer.DefaultInstance.GetHashCode(this);
        }


        // TypeTag cast 
        public static implicit operator Atom(TypeTag type) {
            return new Atom(type);
        }

        public static implicit operator TypeTag(Atom atom) {
            return atom.typetag;
        }

        // Int32 cast
        public static implicit operator Atom(int value) {
            return new Atom(value);
        }

        public static explicit operator int(Atom atom) {
            if (atom.typetag != TypeTag.OscInt32) throw new InvalidCastException();
            return atom.int32value;
        }

        // Float32 cast
        public static implicit operator Atom(float value) {
            return new Atom(value);
        }

        public static explicit operator float(Atom atom) {
            if (atom.typetag != TypeTag.OscFloat32) throw new InvalidCastException();
            return atom.float32value;
        }

        // String cast
        public static implicit operator Atom(string value) {
            return new Atom(value);
        }

        public static explicit operator string(Atom atom) {
            if (atom.typetag != TypeTag.OscString) throw new InvalidCastException();
            return atom.objvalue as string;
        }

        // Blob/binary cast
        public static implicit operator Atom(byte[] value) {
            return new Atom(value);
        }

        public static explicit operator byte[](Atom atom) {
            if (atom.typetag != TypeTag.OscBlob) throw new InvalidCastException();
            return atom.objvalue as byte[];
        }
    }
#if HIDE

    [StructLayout(LayoutKind.Explicit)]
    public struct OldAtom
    {
        // Fields
        [FieldOffset(0)]
        private TypeTag typetag;

        [FieldOffset(4)]
        private OscInt32 int32value;

        [FieldOffset(4)]
        private OscFloat32 float32value;

        [FieldOffset(4)]
        private OscString stringvalue;

        [FieldOffset(4)]
        private OscBlob blobvalue;

        // Properties
        public TypeTag TypeTag {
            get { return typetag; }
            set {
                typetag = value;
            }
        }

        public OscInt32 Int32Value {
            get { return int32value; }
            set {
                int32value = value;
                TypeTag = TypeTag.OscInt32;
            }
        }

        public OscFloat32 Float32Value {
            get { return float32value; }
            set {
                float32value = value;
                TypeTag = TypeTag.OscFloat32;
            }
        }

        public OscString StringValue {
            get { return stringvalue; }
            set {
                stringvalue = value;
                TypeTag = TypeTag.OscString;
            }
        }

        public OscBlob BlobValue {
            get { return blobvalue; }
            set {
                blobvalue = value;
                TypeTag = TypeTag.OscBlob;
            }
        }

        // Ctors 
        public OldAtom(TypeTag type) {
            typetag = type;
            float32value = OscFloat32.Empty;
            stringvalue = OscString.Empty;
            blobvalue = OscBlob.Empty;
            int32value = OscInt32.Empty;
        }

        public OldAtom(int value) {
            float32value = OscFloat32.Empty;
            stringvalue = OscString.Empty;
            blobvalue = OscBlob.Empty;
            int32value = (OscInt32)value;
            typetag = TypeTag.OscInt32;
        }

        public OldAtom(OscInt32 value) {
            float32value = OscFloat32.Empty;
            stringvalue = OscString.Empty;
            blobvalue = OscBlob.Empty;
            int32value = value;
            typetag = TypeTag.OscInt32;
        }

        public OldAtom(float value) {
            stringvalue = OscString.Empty;
            blobvalue = OscBlob.Empty;
            int32value = OscInt32.Empty;
            float32value = (OscFloat32)value;
            typetag = TypeTag.OscFloat32;
        }

        public OldAtom(OscFloat32 value) {
            stringvalue = OscString.Empty;
            blobvalue = OscBlob.Empty;
            int32value = OscInt32.Empty;
            float32value = value;
            typetag = TypeTag.OscFloat32;
        }

        public OldAtom(string value) {
            float32value = OscFloat32.Empty;
            blobvalue = OscBlob.Empty;
            int32value = OscInt32.Empty;
            stringvalue = (OscString)value;
            typetag = TypeTag.OscString;
        }

        public OldAtom(OscString value) {
            float32value = OscFloat32.Empty;
            blobvalue = OscBlob.Empty;
            int32value = OscInt32.Empty;
            stringvalue = value;
            typetag = TypeTag.OscString;
        }

        public OldAtom(byte[] value) {
            stringvalue = OscString.Empty;
            int32value = OscInt32.Empty;
            float32value = OscFloat32.Empty;
            blobvalue = (OscBlob)value;
            typetag = TypeTag.OscBlob;
        }

        public OldAtom(OscBlob value) {
            stringvalue = OscString.Empty;
            int32value = OscInt32.Empty;
            float32value = OscFloat32.Empty;
            blobvalue = value;
            typetag = TypeTag.OscBlob;
        }


        // Methods
        public override string ToString() {
            switch (typetag) {
                case net.TypeTag.OscInt32:
                    return int32value.ToString();
                case net.TypeTag.OscFloat32:
                    return float32value.ToString();
                case net.TypeTag.OscString:
                    return stringvalue.ToString();
                case net.TypeTag.OscBlob:
                    return blobvalue.ToString();
                default:
                    return "unknown";
            }
        }


        // TypeTag cast 
        public static implicit operator OldAtom(TypeTag type) {
            return new OldAtom(type);
        }

        public static implicit operator TypeTag(OldAtom atom) {
            return atom.typetag;
        }

        // Int32 cast
        public static implicit operator OldAtom(OscInt32 value) {
            return new OldAtom(value);
        }

        public static implicit operator OldAtom(int value) {
            return new OldAtom((OscInt32)value);
        }

        public static explicit operator OscInt32(OldAtom atom) {
            if (atom.typetag != TypeTag.OscInt32) throw new InvalidCastException();
            return atom.int32value;
        }

        public static explicit operator int(OldAtom atom) {
            if (atom.typetag != TypeTag.OscInt32) throw new InvalidCastException();
            return atom.int32value.Value;
        }

        // Float32 cast
        public static implicit operator OldAtom(OscFloat32 value) {
            return new OldAtom(value);
        }

        public static implicit operator OldAtom(float value) {
            return new OldAtom((OscFloat32)value);
        }

        public static explicit operator OscFloat32(OldAtom atom) {
            if (atom.typetag != TypeTag.OscFloat32) throw new InvalidCastException();
            return atom.float32value;
        }

        public static explicit operator float(OldAtom atom) {
            if (atom.typetag != TypeTag.OscFloat32) throw new InvalidCastException();
            return atom.float32value.Value;
        }

        // String cast
        public static implicit operator OldAtom(OscString value) {
            return new OldAtom(value);
        }

        public static implicit operator OldAtom(string value) {
            return new OldAtom((OscString)value);
        }

        public static explicit operator OscString(OldAtom atom) {
            if (atom.typetag != TypeTag.OscString) throw new InvalidCastException();
            return atom.stringvalue;
        }

        public static explicit operator string(OldAtom atom) {
            if (atom.typetag != TypeTag.OscString) throw new InvalidCastException();
            return atom.stringvalue.Value;
        }

        // Blob/binary cast
        public static implicit operator OldAtom(OscBlob value) {
            return new OldAtom(value);
        }

        public static implicit operator OldAtom(byte[] value) {
            return new OldAtom((OscBlob)value);
        }

        public static explicit operator OscBlob(OldAtom atom) {
            if (atom.typetag != TypeTag.OscBlob) throw new InvalidCastException();
            return atom.blobvalue;
        }

        public static explicit operator byte[](OldAtom atom) {
            if (atom.typetag != TypeTag.OscBlob) throw new InvalidCastException();
            return atom.blobvalue.Value;
        }
    }

    public interface IOscAtomValue
    {
        object AtomValue { get; }
        TypeTag GetTypeTag();
    }

    public interface IOscAtomValue<T> : IOscAtomValue
    {
        T Value { get; set; }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct OscInt32 : IOscAtomValue<int>
    {
        public static OscInt32 Empty = new OscInt32();
        private int value;

        object IOscAtomValue.AtomValue { get { return Value; } }
        public int Value { get { return value; } set { this.value = value; } }
        public TypeTag GetTypeTag() { return TypeTag.OscInt32; }

        public OscInt32(int value) { this.value = value; }

        public override string ToString() {
            return Value.ToString();
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(obj, null)) return false;
            if (!( obj is OscInt32 )) return false;
            return Equals((OscInt32)obj);
        }

        public bool Equals(OscInt32 rhs) {
            return this.Value == rhs.Value;
        }

        public override int GetHashCode() {
            return Value.GetHashCode();
        }
        
        public static implicit operator int(OscInt32 value) {
            return value.Value;
        }

        public static explicit operator OscInt32(int value) {
            return new OscInt32(value);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct OscFloat32 : IOscAtomValue<float>
    {
        public static OscFloat32 Empty = new OscFloat32();
        private float value;

        object IOscAtomValue.AtomValue { get { return Value; } }
        public float Value { get { return value; } set { this.value = value; } }
        public TypeTag GetTypeTag() { return TypeTag.OscFloat32; }

        public OscFloat32(float value) { this.value = value; }

        public override string ToString() {
            return Value.ToString();
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(obj, null)) return false;
            if (!( obj is OscFloat32 )) return false;
            return Equals((OscFloat32)obj);
        }

        public override int GetHashCode() {
            return Value.GetHashCode();
        }

        public bool Equals(OscFloat32 rhs) {
            return this.Value == rhs.Value;
        }

        public static implicit operator float(OscFloat32 value) {
            return value.Value;
        }

        public static explicit operator OscFloat32(float value) {
            return new OscFloat32(value);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct OscString : IOscAtomValue<string>
    {
        public static OscString Empty = new OscString();

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType=UnmanagedType.U2)]
        private string value;

        object IOscAtomValue.AtomValue { get { return Value; } }
        public string Value { get { return value; } set { this.value = value; } }
        public TypeTag GetTypeTag() { return TypeTag.OscString; }

        public override string ToString() {
            return Value == null ? "null" : Value.ToString();
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(obj, null)) return false;
            if (!( obj is OscString )) return false;
            return Equals((OscString)obj);
        }

        public bool Equals(OscString rhs) {
            return this.Value == rhs.Value;
        }

        public override int GetHashCode() {
            return Value == null ? base.GetHashCode() : Value.GetHashCode();
        }

        public OscString(string value) { this.value = value; }
        public OscString(char[] value) { this.value = new string(value); }
        
        public unsafe OscString(sbyte[] value) { 
            if (value == null) {
                this.value = null;
            }
            else {
                fixed(sbyte* ptr = value) {
                    this.value = new string(ptr);
                }
            }
        }
        
        public unsafe OscString(byte[] value) {
            if (value == null) {
                this.value = null;
            }
            else {
                fixed(byte* ptr = value) {
                    sbyte* p = (sbyte*)ptr;
                    this.value = new string(p);
                }
            }
        }

        public static implicit operator string(OscString value) {
            return value.Value;
        }

        public static explicit operator OscString(string value) {
            return new OscString(value);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct OscBlob : IOscAtomValue<byte[]>
    {
        public static OscBlob Empty = new OscBlob();

        [MarshalAs(UnmanagedType.ByValArray)]
        private byte[] value;

        object IOscAtomValue.AtomValue { get { return Value; } }
        public byte[] Value { get { return value; } set { this.value = value; } }
        public TypeTag GetTypeTag() { return TypeTag.OscBlob; }

        public override string ToString() {
            return Value == null ? "null" : Value.ToString();
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(obj, null)) return false;
            if (!( obj is OscBlob )) return false;
            return Equals((OscBlob)obj);
        }

        public bool Equals(OscBlob rhs) {
            if (this.Value == null) return rhs.Value == null;
            else if (rhs.Value == null) return false;
            else if (this.Value == rhs.Value) return true;
            else if (this.Value.Length != rhs.Value.Length) return false;

            for (int i = 0; i < this.Value.Length; i++) {
                if (this.Value[i] != rhs.Value[i]) return false;
            }

            return true;
        }

        public override int GetHashCode() {
            return Value == null ? base.GetHashCode() : Value.GetHashCode();
        }

        public OscBlob(byte[] value) { this.value = value; }

        public OscBlob(sbyte[] value) {
            if (value == null) {
                this.value = null;
            }
            else {
                this.value = new byte[value.Length];
                for (int i = 0; i < value.Length; i++) {
                    this.value[i] = (byte)value[i];
                }
            }
        }

        public OscBlob(char[] value) {
            if (value == null) {
                this.value = null;
            }
            else {
                this.value = new byte[value.Length];
                for (int i = 0; i < value.Length; i++) {
                    this.value[i] = (byte)value[i];
                }
            }
        }

        public static implicit operator byte[](OscBlob value) {
            return value.Value;
        }

        public static explicit operator OscBlob(byte[] value) {
            return new OscBlob(value);
        }
    }

#endif
}
