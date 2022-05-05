using System.Text;

namespace jug {
    public enum Nat { Obj, Arr, Str, Pri };
    public struct Ent {
        public Nat nat;
        public int size;

        public string data;
        public int start;
        public int len;

        public static Ent Obj(int size) {
            return new Ent { nat = Nat.Obj, size = size };
        }

        public static Ent Arr(int size) {
            return new Ent { nat = Nat.Arr, size = size };
        }

        public static Ent Str(string str) {
            if (str == null) return Ent.Pri("null");
            return new Ent { nat = Nat.Str, data = str, len = str.Length };
        }

        public static Ent Pri(string pri) {
            if (pri == null) pri = "null";
            return new Ent { nat = Nat.Pri, data = pri, len = pri.Length };
        }
    }

    public class Jug {
        private StringBuilder builder;

        public Jug() {
            builder = new StringBuilder();
        }

        public string Gen(int start, Ent[] ents, int indent) {
            builder.Clear();
            if (ents == null || start < 0 || start >= ents.Length) return "{}";
            PriGen(start, ents, indent, 0);
            return builder.ToString();
        }

        private int PriGen(int st, Ent[] ents, int ind, int depth) {
            var parent = Nat.Pri;
            if (st > 0) parent = ents[st - 1].nat;
            int i = st, j = 0, len = 1;
            if (st > 0) len = ents[st - 1].size;
            while (j < len && i < ents.Length) {
                if (parent == Nat.Arr && j != 0) builder.Append(',');
                if (ind > 0 && parent == Nat.Arr) Indent(depth, ind);
                if (parent == Nat.Obj && j % 2 == 0 && j != 0) builder.Append(',');
                if (ind > 0 && parent == Nat.Obj && j % 2 == 0) Indent(depth, ind);
                var ent = ents[i];
                switch (ent.nat) {
                    case Nat.Obj:
                    case Nat.Arr:
                        var nat = ent.nat;
                        if (nat == Nat.Arr) builder.Append('['); else builder.Append('{');
                        i = PriGen(i + 1, ents, ind, depth + 1);
                        if (ind > 0) Indent(depth, ind);
                        if (nat == Nat.Arr) builder.Append(']'); else builder.Append('}');
                        break;
                    case Nat.Str:
                    case Nat.Pri:
                        if (ent.nat == Nat.Str) builder.Append('"');
                        if (ent.data != null) builder.Append(ent.data, ent.start, ent.len);
                        if (ent.nat == Nat.Str) builder.Append('"');
                        i++;
                        break;
                }
                if (parent == Nat.Obj && j % 2 == 0) {
                    builder.Append(':');
                    if (ind > 0) builder.Append(' ');
                }
                j++;
            }

            return i;
        }

        private void Indent(int depth, int p) {
            builder.Append('\n');
            for (int i = 0; i < depth; i++) for (int j = 0; j < p; j++) builder.Append(' ');
        }
    }
}
