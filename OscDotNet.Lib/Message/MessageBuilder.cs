using System;
using System.Collections.Generic;
using System.Text;

namespace OscDotNet.Lib
{
    public class MessageBuilder
    {
        private string address = "/";
        private List<Atom> atoms = new List<Atom>();

        public string GetAddress() {
            return address;
        }

        public void SetAddress(string address) {
            if (address == null) throw new ArgumentNullException("address");
            else if (address.Length == 0) throw new ArgumentException("address cannot be empty.", "address");
            else if (address[0] != '/') throw new ArgumentException("address must begin with a forward-slash ('/').", "address");
            
            this.address = address;
        }

        public int AtomCount {
            get { return atoms.Count; }
        }

        public Atom GetAtom(int index) {
            return atoms[index];
        }

        public void PushAtom(Atom atom) {
            this.atoms.Add(atom);
        }

        public void PushAtom(int value) {
            this.atoms.Add(new Atom(value));
        }

        public void PushAtom(long value) {
            this.atoms.Add(new Atom(value));
        }

        public void PushAtom(float value) {
            this.atoms.Add(new Atom(value));
        }

        public void PushAtom(double value) {
            this.atoms.Add(new Atom(value));
        }

        public void PushAtom(string value) {
            this.atoms.Add(new Atom(value));
        }

        public void PushAtom(byte[] value) {
            this.atoms.Add(new Atom(value));
        }

        public Atom PopAtom() {
            if (this.atoms.Count == 0) {
                throw new InvalidOperationException("No Atom to pop.");
            }

            var idx = this.atoms.Count - 1;
            var atom = this.atoms[idx];
            this.atoms.RemoveAt(idx);
            return atom;
        }

        public void SetAtom(int index, int value) {
            this.atoms[index] = new Atom(value);
        }

        public void SetAtom(int index, float value) {
            this.atoms[index] = new Atom(value);
        }

        public void SetAtom(int index, string value) {
            this.atoms[index] = new Atom(value);
        }

        public void SetAtom(int index, byte[] value) {
            this.atoms[index] = new Atom(value);
        }

        public void Reset() {
            address = "/";
            atoms.Clear();
        }

        public Message ToMessage() {
            var typetags = new TypeTag[atoms.Count];
            
            for (int i = 0; i < atoms.Count; i++) {
                typetags[i] = atoms[i].TypeTag;
            }

            return new Message {
                Address = address,
                TypeTags = typetags,
                Atoms = atoms.ToArray()
            };
        }
    }
}
