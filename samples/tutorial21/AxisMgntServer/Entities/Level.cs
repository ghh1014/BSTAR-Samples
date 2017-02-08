using WallE.Server.Core;

namespace AxisMgntServer.Entities
{
    public class Level : EntityBase
    {
        public const string TableName = "Pl_Axis_LevelRegion";
        public const string ColId = "Id";
        public const string ColName = "Name";
        public const string ColStartElevation = "StartElevation";
        public const string ColEndElevation = "EndElevation";

        #region Properties
        public int Id { get; set; }

        public string Name { get; set; }

        public double StartElevation { get; set; }

        public double EndElevation { get; set; }
        #endregion

        public override string MapPropertyName(string propertyName)
        {
            return propertyName;
        }
    }
}
