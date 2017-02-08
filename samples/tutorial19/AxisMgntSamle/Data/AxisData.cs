using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using AxisMgntSample.Entities;
using WallE.Core;
using WallE.Core.Extend;
using WallE.Core.Models;

namespace AxisMgntSample.Data
{
    internal class AxisData : ProjDataBase
    {
        #region 定义
        public class AxisDataFactory : ProjDataFactory<AxisData>
        {
            protected override AxisData New(ProjectItem projectItem)
            {
                return new AxisData(projectItem);
            }
        }

        public static AxisDataFactory Factory = new AxisDataFactory();

        private AxisData(ProjectItem projItem)
            : base(projItem)
        {
            _axisHub = InitRecordHub(Axis.TableName);
            _levelHub = InitRecordHub(Level.TableName);
        }
        #endregion

        private readonly IRecordHub _axisHub;
        private readonly IRecordHub _levelHub;
        private LoadState _loadState;

        public event EventHandler<StatusChangedEventArgs> AxisRecordStatusChanged;
        public event EventHandler<StatusChangedEventArgs> LevelRecordStatusChanged;

        protected async Task<Result> LoadDataAsync(INotifier notifier)
        {
            if (_loadState != LoadState.Unload)
                return Result.Ok;
            _loadState = LoadState.Loading;
            try
            {
                var r = await _axisHub.LoadAllAsync(notifier);
                if (r.ResultType != ResultType.Ok)
                    throw r.Error;
                r = await _levelHub.LoadAllAsync(notifier);
                if (r.ResultType != ResultType.Ok)
                    throw r.Error;
                _loadState = LoadState.Loaded;
                return Result.Ok;
            }
            catch (Exception e)
            {
                _axisHub.Dispose();
                _levelHub.Dispose();
                _loadState = LoadState.Unload;
                return new Result(e);
            }
        }

        protected override Task<Result> OnLoadingProjectItemAsync(INotifier notifier)
        {
            if (AxisRecordStatusChanged != null)
                AxisRecordStatusChanged(this, null);
            return base.OnLoadingProjectItemAsync(notifier);
        }

        protected override Task<Result> OnUnloadingProjectItemAsync(INotifier notifier)
        {
            //卸载子项目时关闭面板
            AxisPluginService.Ins.CloseDockingPane(ProjectItem.Key);
            return base.OnUnloadingProjectItemAsync(notifier); 
        }

        /// <summary>
        /// Handles the <see cref="E:RecordHubStatusChanged" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="StatusChangedEventArgs"/> instance containing the event data.</param>
        protected override void OnRecordHubStatusChanged(object sender, StatusChangedEventArgs e)
        {
            base.OnRecordHubStatusChanged(sender, e);
            var hub = sender as IRecordHub;
            if (hub == _axisHub)
            {
                if (AxisRecordStatusChanged != null)
                    AxisRecordStatusChanged(this, e);
                return;
            }
            if (hub == _levelHub)
            {
                if (LevelRecordStatusChanged != null)
                    LevelRecordStatusChanged(this, e);
            }
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.PropertyName == "IsDirty")
                AxisPluginService.Ins.UpdateHeaderStatus(ProjectItem.Key, IsDirty);
        }

        #region 轴线信息

        /// <summary>
        /// 加载所有轴线信息
        /// </summary>
        /// <returns>Task&lt;ICollection&lt;IDynamicRecord&gt;&gt;.</returns>
        internal async Task<ICollection<IDynamicRecord>> GetAllAxisAsync()
        {
            var r = await LoadDataAsync(null);
            if (r.ResultType != ResultType.Ok)
                LoggerHelper.Error("加载轴线数据失败！", r.Error);
            return _axisHub.Records.ToList();
        }

        /// <summary>
        /// 删除所有轴线信息
        /// </summary>
        /// <returns></returns>
        internal async Task<Result> DeleteAllAxisAsync()
        {
            return await _axisHub.DeleteManyAsync(_axisHub.Records);
        }

        /// <summary>
        /// 新建轴线信息
        /// </summary>
        /// <returns>Task&lt;IDynamicRecord&gt;.</returns>
        internal async Task<IDynamicRecord> CreateAxisAsync(Axis axis)
        {
            return await _axisHub.NewRecordAsync(axis.ToDictionary(), true);
        }

        /// <summary>
        /// 新建轴线信息
        /// </summary>
        /// <returns></returns>
        internal async Task<List<IDynamicRecord>> CreateAxisAsync(List<Axis> axisList)
        {
            return await _axisHub.NewRecordsAsync(axisList.Select(t => t.ToDictionary()).ToList(), true);
        }

        /// <summary>
        /// 修改轴线记录
        /// </summary>
        /// <param name="record">原始记录</param>
        /// <param name="modifyRecord">修改后的信息字典</param>
        /// <returns></returns>
        internal async Task<Result> ModifyAxisAsync(IDynamicRecord record, Dictionary<string, object> modifyRecord)
        {
            if (record == null)
                return Result.Ok;
            return await _axisHub.ModifyRecordAsync(record, modifyRecord);
        }

        #endregion

        #region 标高区域

        /// <summary>
        /// 获取所有的标高区域
        /// </summary>
        /// <returns>Task&lt;ICollection&lt;IDynamicRecord&gt;&gt;.</returns>
        internal async Task<ICollection<IDynamicRecord>> GetAllLevelAsync()
        {
            var r = await LoadDataAsync(null);
            if (r.ResultType != ResultType.Ok)
                LoggerHelper.Error("加载轴线数据失败！", r.Error);
            return _levelHub.Records.ToList();
        }

        /// <summary>
        /// 删除所有标高信息
        /// </summary>
        /// <returns></returns>
        internal async Task<Result> DeleteAllLevelAsync()
        {
            return await _levelHub.DeleteManyAsync(_levelHub.Records);
        }

        /// <summary>
        /// 新建标高区域
        /// </summary>
        /// <param name="level">标高区域</param>
        /// <returns></returns>
        internal async Task<IDynamicRecord> CreateLevelAsync(Level level)
        {
            return await _levelHub.NewRecordAsync(level.ToDictionary(), true);
        }

        /// <summary>
        /// 新建标高区域
        /// </summary>
        /// <param name="levels">标高区域</param>
        /// <returns></returns>
        internal async Task<List<IDynamicRecord>> CreateLevelAsync(List<Level> levels)
        {
            return await _levelHub.NewRecordsAsync(levels.Select(t => t.ToDictionary()).ToList(), true);
        }

        /// <summary>
        /// 修改标高区域记录
        /// </summary>
        /// <param name="record">原始记录</param>
        /// <param name="modifyRecord">修改后的信息字典</param>
        /// <returns></returns>
        internal async Task<Result> ModifyLevelAsync(IDynamicRecord record, Dictionary<string, object> modifyRecord)
        {
            if (record == null)
                return Result.Ok;
            return await _levelHub.ModifyRecordAsync(record, modifyRecord);
        }

        #endregion
    }
}
