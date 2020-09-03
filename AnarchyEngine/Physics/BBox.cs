using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnarchyEngine.DataTypes;
using Jitter.LinearMath;

namespace AnarchyEngine.Physics {
    public struct BBox {
        public Vector3 Min, Max;

        public Vector3[] Points => new[] {
            Min,
            new Vector3(Max.X, Min.Y, Min.Z),
            new Vector3(Min.X, Max.Y, Min.Z),
            new Vector3(Max.X, Max.Y, Min.Z),
            new Vector3(Min.X, Min.Y, Max.Z),
            new Vector3(Max.X, Min.Y, Max.Z),
            new Vector3(Min.X, Max.Y, Max.Z),
            Max
        };

        public Vector3 Center {
            get => (Max + Min) * .5f;
            set {
                Vector3 diff = Center - value;
                Min += diff;
                Max += diff;
            }
        }

        public Vector3 Size {
            get => Max - Min;
            set {
                Vector3 diff = (value - Size) * .5f;
                Min -= diff;
                Max += diff;
            }
        }

        public BBox(Vector3 min, Vector3 max) {
            Min = min;
            Max = max;
        }

        public void Expand(float amount) {
            var v = new Vector3(amount);
            Min -= v;
            Max += v;
        }

        public static implicit operator JBBox(BBox b) => new JBBox(b.Min, b.Max);
        public static implicit operator BBox(JBBox b) => new BBox(b.Min, b.Max);
    }
}
