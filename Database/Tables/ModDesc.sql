CREATE TABLE [dbo].[ModDesc]
(
    FoodCode INT,
    ModificationCode INT,
    VersionID INT,
    StartDT DATETIME2 NULL,
    EndDT DATETIME2 NULL,
    ModificationDescription VARCHAR(240) NOT NULL,
    CreateDT DATETIME2
        DEFAULT GETUTCDATE() NOT NULL,
    CONSTRAINT PK_ModDesc
        PRIMARY KEY
        (
            ModificationCode,
            VersionID
        )
)
