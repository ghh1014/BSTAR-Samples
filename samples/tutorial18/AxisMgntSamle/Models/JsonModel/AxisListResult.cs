using System.Collections.Generic;
using AxisMgntSample.Entities;
using WallE.Core;

namespace AxisMgntSample.Models
{
    public class AxisListResult : ApiResult
    {
        public AxisListResult()
        {
            ItemInfos = new List<Axis>();
        }

        /// <summary>
        /// 数据记录
        /// </summary>
        public List<Axis> ItemInfos { get; set; }
    }
}
