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
using AxisMgntSample.Entities;
using WallE.Core;

namespace AxisMgntSample.Models
{
    /// <summary>
    /// 轴线界面绑定Model
    /// </summary>
    public class AxisInfo : NotifyObject
    {
        /// <summary>
        /// 构造方法，传入IDynamicRecord
        /// </summary>
        public AxisInfo(IDynamicRecord record)
        {
            AxisRecord = record;
            InitAxisInfo();
            Key = Guid.NewGuid().ToString();
            AxisRecord.PropertyChanged -= AxisRecord_PropertyChanged;
            //监听IDynamicRecord属性更改事件
            AxisRecord.PropertyChanged += AxisRecord_PropertyChanged;
        }

        public AxisInfo() { }

        /// <summary>
        /// IDynamicRecord属性更改需要执行的方法
        /// </summary>
        private void AxisRecord_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            InitAxisInfo();
        }

        /// <summary>
        /// 属性赋值操作
        /// </summary>
        private void InitAxisInfo()
        {
            Id = (int)AxisRecord[Axis.ColId];
            Name = (string)AxisRecord[Axis.ColName];
            Type = (AxisType)(int)AxisRecord[Axis.ColType];
            X1 = (double)AxisRecord[Axis.ColX1];
            Y1 = (double)AxisRecord[Axis.ColY1];
            Z1 = (double)AxisRecord[Axis.ColZ1];
            X2 = (double)AxisRecord[Axis.ColX2];
            Y2 = (double)AxisRecord[Axis.ColY2];
            Z2 = (double)AxisRecord[Axis.ColZ2];
        }

        public IDynamicRecord AxisRecord { get; private set; }

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

        private AxisType _type;
        public AxisType Type
        {
            get { return _type; }
            set { Set("Type", ref _type, value); }
        }

        private string _key;
        public string Key
        {
            get { return _key; }
            set { Set("Key", ref _key, value); }
        }

        private double _x1;
        public double X1
        {
            get { return _x1; }
            set { Set("X1", ref _x1, value); }
        }

        private double _y1;
        public double Y1
        {
            get { return _y1; }
            set { Set("Y1", ref _y1, value); }
        }

        private double _z1;
        public double Z1
        {
            get { return _z1; }
            set { Set("Z1", ref _z1, value); }
        }

        private double _x2;
        public double X2
        {
            get { return _x2; }
            set { Set("X2", ref _x2, value); }
        }

        private double _y2;
        public double Y2
        {
            get { return _y2; }
            set { Set("Y2", ref _y2, value); }
        }

        private double _z2;
        public double Z2
        {
            get { return _z2; }
            set { Set("Z2", ref _z2, value); }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { Set("IsSelected", ref _isSelected, value); }
        }
    }
}