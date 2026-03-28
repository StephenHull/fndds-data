ALTER TABLE [dbo].ModEquiv
ADD CONSTRAINT FK_ModEquiv_M0odDesc
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
