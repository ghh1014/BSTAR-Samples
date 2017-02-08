// ***********************************************************************
// Assembly         : AxisMgnt.dll
// Author           : 
// Created          : 2016-03-25 9:26
// 
// Last Modified By : 郭华华
// Last Modified On : 2016-03-25 10:13
// ***********************************************************************
// <copyright file="AxisInfo.cs" company="深圳筑星科技有限公司">
//      Copyright (c) BStar All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using AxisMgntSample.Data;
using AxisMgntSample.Entities;
using WallE.Core;
using WallE.Core.Extend;

namespace AxisMgntSample.Models
{
    /// <summary>
    /// Class AxisInfo.
    /// </summary>
    public class LevelInfo : NotifyObject
    {
        public LevelInfo(IDynamicRecord record)
        {
            LevelRecord = record;
            EndNameEdit = OnEndNameEdit;
            InitLevelInfo();
            LevelRecord.PropertyChanged -= LevelRecord_PropertyChanged;
            LevelRecord.PropertyChanged += LevelRecord_PropertyChanged;
        }

        internal AxisData AxisData { get; set; }

        private void LevelRecord_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            InitLevelInfo();
        }

        private void InitLevelInfo()
        {
            Id = (int)LevelRecord[Level.ColId];
            Name = (string)LevelRecord[Level.ColName];
            StartElevation = (double)LevelRecord[Level.ColStartElevation];
            EndElevation = (double)LevelRecord[Level.ColEndElevation];
        }

        public IDynamicRecord LevelRecord { get; set; }

        private int _id;
        public int Id
        {
            get { return _id; }
            set { Set("Id", ref _id, value); }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { Set("Name", ref _name, value); }
        }

        private double _startElevation;
        public double StartElevation
        {
            get { return _startElevation; }
            set { Set("StartElevation", ref _startElevation, value); }
        }

        private double _endElevation;
        public double EndElevation
        {
            get { return _endElevation; }
            set { Set("EndElevation", ref _endElevation, value); }
        }

        #region 更新名称

        public Action<string> EndNameEdit { get; private set; }

        private async void OnEndNameEdit(string name)
        {
            if (string.IsNullOrEmpty(name))
                return;
            var curName = Name;
            if (name == curName)
                return;
            var r = await AxisData.ModifyLevelAsync(LevelRecord, new Dictionary<string, object> {{Level.ColName, name}});
            if (r.ResultType!=ResultType.Ok)
            {
                LoggerHelper.Error("修改层区域名称失败！",r.Error);
            }
        }

        #endregion
    }
}