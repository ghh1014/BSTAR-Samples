using System.Collections.Generic;
using AxisMgntSample.Entities;
using WallE.Core;

namespace AxisMgntSample.Models
{
    public class LevelsResult : ApiResult
    {
        public LevelsResult()
        {
            ItemInfos = new List<Level>();
        }

        /// <summary>
        /// 数据记录
        /// </summary>
        public List<Level> ItemInfos { get; set; }
    }
}
