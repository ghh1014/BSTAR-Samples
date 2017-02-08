using System;
using System.Collections.Generic;
using AxisMgntServer.Entities;
using WallE.Server.Core;

namespace AxisMgntServer.Models
{
    public class AxisListResult : ApiResult
    {
        public AxisListResult(List<Axis> infos)
        {
            IsSuccess = true;
            ItemInfos = infos;
        }

        public AxisListResult(Exception exception) : base(exception)
        {
        }

        /// <summary>
        /// 数据记录
        /// </summary>
        public List<Axis> ItemInfos { get; set; }
    }
}
