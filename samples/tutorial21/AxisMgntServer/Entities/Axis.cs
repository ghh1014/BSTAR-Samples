using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WallE.Server.Core;

namespace AxisMgntServer.Entities
{
    public class Axis : EntityBase
    {
        public const string TableName = "Pl_Axis_AxisInfo";
        public const string ColId = "Id";
        public const string ColName = "Name";
        public const string ColX1 = "X1";
        public const string ColY1 = "Y1";
        public const string ColZ1 = "Z1";
        public const string ColX2 = "X2";
        public const string ColY2 = "Y2";
        public const string ColZ2 = "Z2";
        public const string ColType = "Type";

        #region Properties
        public int Id { get; set; }

        public string Name { get; set; }

        public double X1 { get; set; }

        public double Y1 { get; set; }

        public double Z1 { get; set; }

        public double X2 { get; set; }

        public double Y2 { get; set; }

        public double Z2 { get; set; }

        public int Type { get; set; }
        #endregion

        public override string MapPropertyName(string propertyName)
        {
            return propertyName;
        }
    }
}
