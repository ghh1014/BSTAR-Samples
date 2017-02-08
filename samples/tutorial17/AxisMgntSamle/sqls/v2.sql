-- ----------------------------
-- Table structure for Pl_Axis_LevelRegion
-- ----------------------------
DROP TABLE [dbo].[Pl_AxisSample_LevelRegion]
GO
CREATE TABLE [dbo].[Pl_AxisSample_LevelRegion] (
[Id] int NOT NULL ,
[Name] varchar(200) NULL ,
[StartElevation] float(53) NULL ,
[EndElevation] float(53) NULL 
)
GO

-- ----------------------------
-- Indexes structure for table Pl_Axis_LevelRegion
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table Pl_Axis_LevelRegion
-- ----------------------------
ALTER TABLE [dbo].[Pl_AxisSample_LevelRegion] ADD PRIMARY KEY ([Id])
GO
