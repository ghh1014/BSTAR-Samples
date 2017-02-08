using System;
using System.Collections.Generic;
using AxisMgntServer.Entities;
using WallE.Server.Core;

namespace AxisMgntServer.Models
{
    public class LevelsResult : ApiResult
    {
        public LevelsResult(List<Level> infos)
        {
            IsSuccess = true;
            ItemInfos = infos;
        }

        public LevelsResult(Exception exception) : base(exception)
        {
        }

        /// <summary>
        /// 数据记录
        /// </summary>
        public List<Level> ItemInfos { get; set; }
    }
}
