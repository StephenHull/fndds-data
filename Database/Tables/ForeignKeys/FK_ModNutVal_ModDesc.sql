ALTER TABLE [dbo].ModNutVal
ADD CONSTRAINT FK_ModNutVal_ModDesc
    FOREIGN KEY
    (
        ModificationCode,
        VersionID
    )
    REFERENCES [dbo].ModDesc
    (
        ModificationCode,
        VersionID
    ) ON DELETE CASCADE
