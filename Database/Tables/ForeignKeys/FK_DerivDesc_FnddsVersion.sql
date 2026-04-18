ALTER TABLE [dbo].DerivDesc
ADD CONSTRAINT FK_DerivDesc_FnddsVersion
    FOREIGN KEY (VersionID)
    REFERENCES [dbo].[FnddsVersion] (ID) ON DELETE CASCADE