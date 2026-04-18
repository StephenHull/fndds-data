ALTER TABLE [dbo].SubcodeDesc
ADD CONSTRAINT FK_SubcodeDesc_FnddsVersion
	FOREIGN KEY (VersionID)
	REFERENCES [dbo].[FnddsVersion] (ID) ON DELETE CASCADE