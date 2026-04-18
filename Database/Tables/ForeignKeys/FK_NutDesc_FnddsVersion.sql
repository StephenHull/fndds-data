ALTER TABLE [dbo].NutDesc
ADD CONSTRAINT FK_NutDesc_FnddsVersion
    FOREIGN KEY (VersionID)
    REFERENCES [dbo].[FnddsVersion] (ID) ON DELETE CASCADE