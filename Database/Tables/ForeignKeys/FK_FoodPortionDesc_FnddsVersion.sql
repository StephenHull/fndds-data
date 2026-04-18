ALTER TABLE [dbo].FoodPortionDesc
ADD CONSTRAINT FK_FoodPortionDesc_FnddsVersion
    FOREIGN KEY (VersionID)
    REFERENCES [dbo].[FnddsVersion] (ID) ON DELETE CASCADE