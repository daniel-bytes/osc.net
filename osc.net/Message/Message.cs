using System;
using System.Collections.Generic;
using System.Text;

namespace osc.net
{
    public class Message : IEnumerable<Atom>
    {
        public string Address { get; set; }
        public TypeTag[] TypeTags { get; set; }
        public Atom[] Atoms { get; set; }

        internal Message() { }

        public override bool Equals(object obj) {
            return base.Equals(obj as Message);
        }

        public virtual bool Equals(Message rhs) {
            if (rhs == null) return false;

            if (this.Address != rhs.Address) return false;
            if (this.Atoms == null) {
                return rhs.Atoms == null;
            }
            else if (rhs.Atoms == null) {
                return false;
            }

            if (this.Atoms.Length != rhs.Atoms.Length) return false;

            for (int i = 0; i < this.Atoms.Length; i++) {
                if (!this.Atoms[i].Equals(rhs.Atoms[i])) {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode() {
            int hashCode = ( Address == null ? 0 : Address.GetHashCode() );
            if (Atoms != null) {
                foreach (var atom in Atoms) {
                    hashCode ^= atom.GetHashCode();
                }
            }

            return hashCode;
        }

        #region IEnumerable<Atom> Members

        public IEnumerator<Atom> GetEnumerator() {
            foreach (var atom in Atoms ?? new Atom[0]) {
                yield return atom;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return this.GetEnumerator();   
        }

        #endregion
    }
}
