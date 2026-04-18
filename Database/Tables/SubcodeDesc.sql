CREATE TABLE [dbo].[SubcodeDesc]
(
    Subcode INT,
    VersionID INT,
    StartDT DATETIME2 NULL,
    EndDT DATETIME2 NULL,
    SubcodeDescription VARCHAR(80) NOT NULL,
    CreateDT DATETIME2
        DEFAULT CURRENT_TIMESTAMP NOT NULL,
    CONSTRAINT PK_SubcodeDesc
        PRIMARY KEY
        (
            Subcode,
            VersionID
        )
)
