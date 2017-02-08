// ***********************************************************************
// Assembly         : AxisMgnt.dll
// Author           : 
// Created          : 2016-03-25 10:08
// 
// Last Modified By : 郭华华
// Last Modified On : 2016-03-25 10:12
// ***********************************************************************
// <copyright file="Level.cs" company="深圳筑星科技有限公司">
//      Copyright (c) BStar All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using WallE.Core.Models;

namespace AxisMgntSample.Entities
{
    public class Level : EntityBase
    {
        public const string TableName = "Pl_AxisSample_LevelRegion";
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