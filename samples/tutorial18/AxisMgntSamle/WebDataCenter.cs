using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AxisMgntSample.Entities;
using AxisMgntSample.Models;
using WallE.Core;

namespace AxisMgntSample
{
    public class WebDataCenter
    {
        private WebDataCenter()
        {

        }
        private static WebDataCenter _ins;
        public static WebDataCenter Ins => _ins ?? (_ins = new WebDataCenter());

        /// <summary>
        /// 通过接口获取标高信息
        /// </summary>
        /// <returns></returns>
        public async Task<List<Level>> GetLevelInfosAsync(string projectItemKey)
        {
            var curResult = await M.NetManager.GetAsync<LevelsResult>("v1/ProjectInfo/GetLevelInfos", new
            {
                userId = M.EnvManager.CurrentUser.Id,
                projectItemKey = projectItemKey
            });
            if (!curResult.IsSuccess || curResult.ItemInfos.Count == 0)
                throw new Exception("获取轴线标高数据失败！");
            return curResult.ItemInfos;
        }

        /// <summary>
        /// 通过接口获取轴网信息
        /// </summary>
        /// <returns></returns>
        public async Task<List<Axis>> GetAxisInfosAsync(string projectItemKey)
        {
            var curResult = await M.NetManager.GetAsync<AxisListResult>("v1/ProjectInfo/GetAxisInfos", new
            {
                userId = M.EnvManager.CurrentUser.Id,
                projectItemKey = projectItemKey
            });
            if (!curResult.IsSuccess || curResult.ItemInfos.Count == 0)
                throw new Exception("获取轴线标高数据失败！");
            return curResult.ItemInfos;
        }
    }
}
