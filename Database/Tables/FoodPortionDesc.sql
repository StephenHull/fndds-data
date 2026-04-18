CREATE TABLE [dbo].[FoodPortionDesc]
(
    PortionCode INT,
    VersionID INT,
    StartDT DATETIME2 NULL,
    EndDT DATETIME2 NULL,
    PortionDescription VARCHAR(120) NOT NULL,
    ChangeType VARCHAR(1) NULL,
    CreateDT DATETIME2
        DEFAULT GETUTCDATE() NOT NULL,
    CONSTRAINT PK_FoodPortionDesc
        PRIMARY KEY
        (
            PortionCode,
            VersionID
        )
)
