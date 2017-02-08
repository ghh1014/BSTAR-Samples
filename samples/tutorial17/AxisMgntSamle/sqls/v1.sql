-- ----------------------------
-- Table structure for Pl_Axis_AxisInfo
-- ----------------------------
DROP TABLE [dbo].[Pl_AxisSample_AxisInfo]
GO
CREATE TABLE [dbo].[Pl_AxisSample_AxisInfo] (
[Id] int NOT NULL ,
[Name] varchar(200) NULL ,
[X1] float(53) NULL ,
[Y1] float(53) NULL ,
[Z1] float(53) NULL ,
[X2] float(53) NULL ,
[Y2] float(53) NULL ,
[Z2] float(53) NULL ,
[Type] int NULL 
)
GO

-- ----------------------------
-- Indexes structure for table Pl_Axis_AxisInfo
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table Pl_Axis_AxisInfo
-- ----------------------------
ALTER TABLE [dbo].[Pl_AxisSample_AxisInfo] ADD PRIMARY KEY ([Id])
GO
