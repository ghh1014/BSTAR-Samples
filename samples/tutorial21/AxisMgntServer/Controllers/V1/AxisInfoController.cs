using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using AxisMgntServer.Entities;
using AxisMgntServer.Models;
using WallE.Server.Core;

namespace AxisMgntServer.Controllers.V1
{
    /// <summary>
    /// 轴线信息管理中心
    /// </summary>
    public class AxisInfoController : ApiController
    {
        /// <summary>
        /// 获取子项目下的区域信息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="userId"></param>
        /// <param name="projectItemKey"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<LevelsResult> GetLevelInfos(string accessToken, string userId, string projectItemKey)
        {
            try
            {
                var dbAccessor = M.DbAccessorMgnt.GetDbAccessorByProjectItemKey(projectItemKey);
                var allRecords = await dbAccessor.AllAsync(Level.TableName);
                var infos = allRecords.Select(t => t.As<Level>()).ToList();
                return new LevelsResult(infos);
            }
            catch (Exception ex)
            {
                return new LevelsResult(ex);
            }
        }

        /// <summary>
        /// 获取子项目下的轴线信息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="userId"></param>
        /// <param name="projectItemKey"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<AxisListResult> GetAxisInfos(string accessToken, string userId, string projectItemKey)
        {
            try
            {
                var dbAccessor = M.DbAccessorMgnt.GetDbAccessorByProjectItemKey(projectItemKey);
                var allRecords = await dbAccessor.AllAsync(Axis.TableName);
                var infos = allRecords.Select(t => t.As<Axis>()).ToList();
                return new AxisListResult(infos);
            }
            catch (Exception ex)
            {
                return new AxisListResult(ex);
            }
        }
    }
}