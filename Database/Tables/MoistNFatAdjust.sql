CREATE TABLE [dbo].[MoistNFatAdjust]
(
    FoodCode INT,
    VersionID INT,
    StartDT DATETIME2 NULL,
    EndDT DATETIME2 NULL,
    MoistureChange DECIMAL(5, 1) NULL,
    FatChange DECIMAL(5, 1) NULL,
    TypeOfFat INT NULL,
    CreateDT DATETIME2
        DEFAULT GETUTCDATE() NOT NULL,
    CONSTRAINT PK_MoistNFatAdjust
        PRIMARY KEY
        (
            FoodCode,
            VersionID
        )
)
