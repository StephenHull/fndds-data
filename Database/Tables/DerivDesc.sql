CREATE TABLE [dbo].[DerivDesc]
(
    DerivationCode VARCHAR(4),
    VersionID INT,
    DerivationDescription VARCHAR(120) NOT NULL,
    CreateDT DATETIME2
        DEFAULT GETUTCDATE() NOT NULL,
    CONSTRAINT PK_DerivDesc
        PRIMARY KEY
        (
            DerivationCode,
            VersionID
        )
)
