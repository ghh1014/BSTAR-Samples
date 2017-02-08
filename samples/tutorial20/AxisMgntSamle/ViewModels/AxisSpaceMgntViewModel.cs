// ***********************************************************************
// Assembly         : AxisMgnt.dll
// Author           :
// Created          : 2016-05-05 9:21
//
// Last Modified By : 郭华华
// Last Modified On : 2016-05-05 16:26
// ***********************************************************************
// <copyright file="AxisSpaceMgntViewModel.cs" company="深圳筑星科技有限公司">
//      Copyright (c) BStar All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;
using AxisMgntSample.Data;
using AxisMgntSample.Entities;
using AxisMgntSample.Models;
using WallE.Assist;
using WallE.Core;
using WallE.Core.Extend;
using WallE.Core.Models;

namespace AxisMgntSample.ViewModels
{
    public class AxisSpaceMgntViewModel : ViewModelBase
    {
        private readonly AxisData _axisData;
        private ObservableCollection<AxisInfo> _axisInfos;
        private bool _isBusy;
        private ObservableCollection<LevelInfo> _spaceLevels;

        public AxisSpaceMgntViewModel(ProjectItem projectItem)
        {
            _axisData = AxisData.Factory.Get(projectItem.Key);
            ImportAxis = new RelayCommand(OnImportAxis, CanImportAxis);
            ClearAllAxis = new RelayCommand(OnClearAllAxis, CanClearAllAxis);
            _axisInfos = new ObservableCollection<AxisInfo>();
            _spaceLevels = new ObservableCollection<LevelInfo>();
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            private set { Set("IsBusy", ref _isBusy, value); }
        }

        public ObservableCollection<AxisInfo> AxisInfos
        {
            get { return _axisInfos; }
            set { Set("AxisInfos", ref _axisInfos, value); }
        }

        public ObservableCollection<LevelInfo> SpaceLevels
        {
            get { return _spaceLevels; }
            set { Set("SpaceLevels", ref _spaceLevels, value); }
        }

        #region Command 命令

        public RelayCommand ImportAxis { get; private set; }

        public RelayCommand ClearAllAxis { get; private set; }

        public async void OnImportAxis()
        {
            await ImportAxisInfoAsync();
        }

        public bool CanImportAxis()
        {
            if (IsBusy)
                return false;
            if ((AxisInfos != null && AxisInfos.Count > 0) ||
                (SpaceLevels != null && SpaceLevels.Count > 0))
                return false;
            return true;
        }

        public async void OnClearAllAxis()
        {
            //判断是否要删除
            var closeResult = this.ShowMessage("确认要清除轴线信息？");
            if (CloseResult.Cancel == closeResult)
                return;
            var r1 = await _axisData.DeleteAllAxisAsync();
            if (r1.ResultType != ResultType.Ok)
            {
                LoggerHelper.Error("无法删除所有轴线信息！", r1.Error);
                return;
            }
            var r2 = await _axisData.DeleteAllLevelAsync();
            if (r2.ResultType != ResultType.Ok)
                LoggerHelper.Error("无法删除所有层区域信息！", r2.Error);
        }

        public bool CanClearAllAxis()
        {
            if (IsBusy)
                return false;
            if ((AxisInfos != null && AxisInfos.Count > 0) ||
                (SpaceLevels != null && SpaceLevels.Count > 0))
                return true;
            return false;
        }

        #endregion Command 命令

        #region 加载数据及数据同步

        /// <summary>
        /// 加载数据
        /// </summary>
        protected override async void OnViewLoaded(bool isFirstLoaded)
        {
            base.OnViewLoaded(isFirstLoaded);
            IsBusy = true;
            _axisInfos.Clear();
            _spaceLevels.Clear();
            var records = await _axisData.GetAllAxisAsync();
            foreach (var record in records)
            {
                var axisInfo = new AxisInfo(record);
                _axisInfos.Add(axisInfo);
            }
            var spaceLevels = new List<LevelInfo>();
            var recordList = await _axisData.GetAllLevelAsync();
            foreach (var record in recordList)
            {
                var levelInfo = new LevelInfo(record) { AxisData = _axisData };
                spaceLevels.Add(levelInfo);
            }
            spaceLevels = spaceLevels.OrderByDescending(t => t.StartElevation).ToList();
            _spaceLevels.AddRange(spaceLevels);
            WeakEventManager<AxisData, StatusChangedEventArgs>.AddHandler(_axisData, "AxisRecordStatusChanged",
                OnAxisRecordStatusChanged);
            WeakEventManager<AxisData, StatusChangedEventArgs>.AddHandler(_axisData, "LevelRecordStatusChanged",
                OnLevelRecordStatusChanged);
            IsBusy = false;
        }

        /// <summary>
        /// 数据同步
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="StatusChangedEventArgs"/> instance containing the event data.</param>
        private void OnLevelRecordStatusChanged(object sender, StatusChangedEventArgs e)
        {
            if (e.Action == StatusChangedAction.Added)
            {
                var records = e.Records;
                var existIds = _spaceLevels.Select(t => t.Id).ToList();
                var hasDataChange = false;
                for (var i = 0; i < records.Count; i++)
                {
                    var record = records[i];
                    if (!existIds.Contains((int)record[Level.ColId]))
                    {
                        var levelInfo = new LevelInfo(record) { AxisData = _axisData };
                        _spaceLevels.Add(levelInfo);
                        hasDataChange = true;
                    }
                }
                if (hasDataChange)
                {
                    var spaceLevels = _spaceLevels.OrderByDescending(t => t.StartElevation).ToList();
                    _spaceLevels.Clear();
                    _spaceLevels.AddRange(spaceLevels);
                }
            }
            else if (e.Action == StatusChangedAction.Deleted)
            {
                var delItems = _spaceLevels.Where(t => e.Records.Contains(t.LevelRecord)).ToList();
                delItems.ForEach(t => _spaceLevels.Remove(t));
            }
            else if (e.Action == StatusChangedAction.Reset)
                SpaceLevels.Clear();
        }

        private void OnAxisRecordStatusChanged(object sender, StatusChangedEventArgs e)
        {
            if (e == null)
                return;
            if (e.Action == StatusChangedAction.Added)
            {
                var records = e.Records;
                var existIds = _axisInfos.Select(t => t.Id).ToList();
                for (var i = 0; i < records.Count; i++)
                {
                    var record = records[i];
                    if (!existIds.Contains((int)record[Axis.ColId]))
                    {
                        var axisInfo = new AxisInfo(record);
                        _axisInfos.Add(axisInfo);
                    }
                }
            }
            else if (e.Action == StatusChangedAction.Deleted)
            {
                var delItems = _axisInfos.Where(t => e.Records.Contains(t.AxisRecord)).ToList();
                delItems.ForEach(t => _axisInfos.Remove(t));
            }
            else if (e.Action == StatusChangedAction.Reset)
            {
                AxisInfos.Clear();
            }
        }

        #endregion

        #region 导入轴线、标高信息

        //导入文件信息
        private async Task<bool> ImportAxisInfoAsync()
        {
            var filePath = FileHelper.ChooseFile("选择文件", "text files|*.txt");
            if (string.IsNullOrEmpty(filePath))
                return false;
            var streamReader = new StreamReader(filePath, Encoding.UTF8);
            var strLine = streamReader.ReadLine();
            var spaceLevelList = new List<Level>();
            var spaceAxisList = new List<Axis>();
            var hasFormatError = false;
            while (strLine != null)
            {
                if (strLine == "")
                {
                    //过滤空行
                    strLine = streamReader.ReadLine();
                    continue;
                }
                if (strLine.Contains("轴网"))
                {
                    strLine = streamReader.ReadLine();
                    while (strLine != null && !strLine.Contains("标高"))
                    {
                        if (!GetAxisInfo(strLine, spaceAxisList))
                            hasFormatError = true;
                        strLine = streamReader.ReadLine();
                    }
                }
                if (strLine != null && strLine.Contains("标高"))
                {
                    strLine = streamReader.ReadLine();
                    while (strLine != null && !strLine.Contains("轴网"))
                    {
                        if (!GetLevelInfo(strLine, spaceLevelList))
                            hasFormatError = true;
                        strLine = streamReader.ReadLine();
                    }
                }
            }
            if (hasFormatError)
            {
                var r = this.ShowMessage("文件数据有误，是否忽略错误！", "警告", MessageBoxButton.YesNo);
                if (r != MessageBoxResult.Yes)
                    return false;
            }
            var vector1 = new Vector();
            var type = false; //开始设置轴线类型
            for (var i = 0; i < spaceAxisList.Count; i++)
            {
                var record = spaceAxisList[i];
                var curVector = new Vector((record.X2 - record.X1), (record.Y2 - record.Y1));
                curVector.Normalize();
                if (i == 0)
                {
                    vector1 = curVector;
                    type = IsExistAlpha(record.Name);
                    record.Type = type ? (int)AxisType.HorizontalAxis : (int)AxisType.VerticalAxis;
                }
                else
                {
                    var result = Vector.Multiply(vector1, curVector);
                    if (Math.Abs(result) < 0.01)
                        record.Type = type ? (int)AxisType.VerticalAxis : (int)AxisType.HorizontalAxis;
                    else
                        record.Type = type ? (int)AxisType.HorizontalAxis : (int)AxisType.VerticalAxis;
                }
            }
            return await AddAxisAndLevelAsync(spaceAxisList, spaceLevelList);
        }

        private async Task<bool> AddAxisAndLevelAsync(List<Axis> axisList, List<Level> levelList)
        {
            var scale = new Vector3D(0.001, -0.001, 0.001);
            var translate = new Vector3D();
            foreach (var axis in axisList)
            {
                if (axis.Type == (int)AxisType.VerticalAxis && axis.Y1 < axis.Y2)
                {
                    //纵轴需要保证Y大的点为第一个点
                    var temp = axis.X1;
                    axis.X1 = axis.X2;
                    axis.X2 = temp;
                    temp = axis.Y1;
                    axis.Y1 = axis.Y2;
                    axis.Y2 = temp;
                    temp = axis.Z1;
                    axis.Z1 = axis.Z2;
                    axis.Z2 = temp;
                }
                if (axis.Type == (int)AxisType.HorizontalAxis && axis.X1 < axis.X2)
                {
                    //横轴需要保证X大的点为第一个点
                    var temp = axis.X1;
                    axis.X1 = axis.X2;
                    axis.X2 = temp;
                    temp = axis.Y1;
                    axis.Y1 = axis.Y2;
                    axis.Y2 = temp;
                    temp = axis.Z1;
                    axis.Z1 = axis.Z2;
                    axis.Z2 = temp;
                }
                axis.X1 = axis.X1 * scale.X + translate.X;
                axis.Y1 = axis.Y1 * scale.Y + translate.Y;
                axis.Z1 = axis.Z1 * scale.Z + translate.Z;
                axis.X2 = axis.X2 * scale.X + translate.X;
                axis.Y2 = axis.Y2 * scale.Y + translate.Y;
                axis.Z2 = axis.Z2 * scale.Z + translate.Z;
            }
            foreach (var level in levelList)
            {
                level.StartElevation = level.StartElevation * scale.Z;
                level.EndElevation = level.EndElevation * scale.Z;
            }
            await _axisData.CreateAxisAsync(axisList);
            await _axisData.CreateLevelAsync(levelList);
            return true;
        }

        public bool IsExistAlpha(string str)
        {
            return Regex.Matches(str, "[a-zA-Z]").Count != 0;
        }

        //判断两个Double是否相等
        public bool CloseTo(double d1, double d2)
        {
            return Math.Abs(d1 - d2) < 0.0001;
        }

        //获取标高信息
        private bool GetLevelInfo(string strLine, ICollection<Level> spaceLevelList)
        {
            var spaceLevelRecord = new Level();
            try
            {
                var strList = strLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (strList.Length < 3)
                    return false;
                spaceLevelRecord.Name = strList[0].Trim();
                var elevation1 = Convert.ToDouble(strList[1]);
                var elevation2 = Convert.ToDouble(strList[2]);
                if (elevation1 < elevation2)
                {
                    spaceLevelRecord.StartElevation = elevation1;
                    spaceLevelRecord.EndElevation = elevation2;
                }
                else
                {
                    spaceLevelRecord.StartElevation = elevation2;
                    spaceLevelRecord.EndElevation = elevation1;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Warn("标高信息格式有误：" + strLine, ex);
                return false;
            }
            spaceLevelList.Add(spaceLevelRecord);
            return true;
        }

        //获取轴线信息
        private bool GetAxisInfo(string strLine, ICollection<Axis> spaceAxisList)
        {
            var spaceAxisRecord = new Axis();
            try
            {
                var stringBuilder = new StringBuilder(strLine);
                stringBuilder.Replace('(', '$');
                stringBuilder.Replace(')', '$');
                var strList = stringBuilder.ToString().Split('$');
                if (strList.Length < 4)
                    return false;
                spaceAxisRecord.Name = strList[0].Trim();
                var strPoint1List = strList[1].Split(',');
                var strPoint2List = strList[3].Split(',');
                spaceAxisRecord.X1 = Convert.ToDouble(strPoint1List[0]);
                spaceAxisRecord.Y1 = Convert.ToDouble(strPoint1List[1]);
                spaceAxisRecord.Z1 = Convert.ToDouble(strPoint1List[2]);
                spaceAxisRecord.X2 = Convert.ToDouble(strPoint2List[0]);
                spaceAxisRecord.Y2 = Convert.ToDouble(strPoint2List[1]);
                spaceAxisRecord.Z2 = Convert.ToDouble(strPoint2List[2]);
            }
            catch (Exception ex)
            {
                LoggerHelper.Warn("轴线信息格式有误：" + strLine, ex);
                return false;
            }
            spaceAxisList.Add(spaceAxisRecord);
            return true;
        }

        #endregion
    }
}