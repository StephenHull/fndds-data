ALTER TABLE [dbo].MainFoodDesc
ADD CONSTRAINT FK_MainFoodDesc_FnddsVersion
    FOREIGN KEY (VersionID)
    REFERENCES [dbo].[FnddsVersion] (ID) ON DELETE CASCADE