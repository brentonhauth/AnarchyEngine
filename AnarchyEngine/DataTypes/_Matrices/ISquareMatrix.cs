using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnarchyEngine.DataTypes {
    public interface ISquareMatrix {
        float Trace { get; }
        float Determinant { get; }
    }
}
