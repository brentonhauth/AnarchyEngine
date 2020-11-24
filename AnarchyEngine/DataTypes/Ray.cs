using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnarchyEngine.DataTypes {
    public struct Ray {
        public Vector3 Origin, Direction;

        public Ray(Vector3 origin, Vector3 direction) {
            Origin = origin;
            Direction = direction;
        }

        public static bool operator ==(Ray left, Ray right) {
            return left.Origin == right.Origin &&
                left.Direction == right.Direction;
        }

        public static bool operator !=(Ray left, Ray right) {
            return left.Origin != right.Origin ||
                left.Direction != right.Direction;
        }

        public override bool Equals(object o) {
            return o is Ray r && this == r;
        }

        public override int GetHashCode() {
            const int n = -1521134295;
            int code = -1708057391;
            code = code * n + Origin.GetHashCode();
            code = code * n + Direction.GetHashCode();
            return code;
        }
    }
}
