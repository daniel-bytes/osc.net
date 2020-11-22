using System;
using System.Collections.Generic;
using System.Text;

namespace OscDotNet.Lib
{
    public class AtomEqualityComparer : IEqualityComparer<Atom>
    {
        public static AtomEqualityComparer DefaultInstance = new AtomEqualityComparer();

        #region IEqualityComparer<Atom> Members

        public bool Equals(Atom x, Atom y) {
            if (x.TypeTag != y.TypeTag) {
                return false;
            }

            switch (x.TypeTag) {
                case TypeTag.OscInt32:
                    return x.Int32Value == y.Int32Value;
                case TypeTag.OscFloat32:
                    return x.Float32Value == y.Float32Value;
                case TypeTag.OscString:
                    return x.StringValue == y.StringValue;
                case TypeTag.OscBlob:
                    return BlobEqualityComparer.DefaultInstance.Equals(x.BlobValue, y.BlobValue);
                default:
                    return false;
            }
        }

        public int GetHashCode(Atom obj) {
            switch (obj.TypeTag) {
                case TypeTag.OscInt32:
                    return obj.Int32Value.GetHashCode();
                case TypeTag.OscFloat32:
                    return obj.Float32Value.GetHashCode();
                case TypeTag.OscString:
                    return ( obj.StringValue == null ? 0 : obj.StringValue.GetHashCode() );
                case TypeTag.OscBlob:
                    return ( obj.BlobValue == null ? 0 : obj.BlobValue.GetHashCode() );
                default:
                    return base.GetHashCode();
            }
        }

        #endregion
    }

    public class BlobEqualityComparer : IEqualityComparer<byte[]>
    {
        public static BlobEqualityComparer DefaultInstance = new BlobEqualityComparer();

        #region IEqualityComparer<byte[]> Members

        public bool Equals(byte[] x, byte[] y) {
            if (x == null) return y == null;
            else if (y == null) return false;

            if (x.Length != y.Length) return false;

            for (int i = 0; i < x.Length; i++) {
                if (x[i] != y[i]) return false;
            }

            return true;
        }

        public int GetHashCode(byte[] obj) {
            return obj == null ? 0 : obj.GetHashCode();
        }

        #endregion
    }
}
