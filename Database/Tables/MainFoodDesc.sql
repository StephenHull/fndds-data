CREATE TABLE [dbo].[MainFoodDesc]
(
    FoodCode INT,
    VersionID INT,
    StartDT DATETIME2 NULL,
    EndDT DATETIME2 NULL,
    MainFoodDescription VARCHAR(200) NOT NULL,
    AbbreviatedMainFoodDescription VARCHAR(60) NULL,
    FortificationIdentifier VARCHAR(2) NULL,
    CategoryNumber INT NULL,
    CategoryDescription VARCHAR(80) NULL,
    CreateDT DATETIME2
        DEFAULT GETUTCDATE() NOT NULL,
    CONSTRAINT PK_MainFoodDesc
        PRIMARY KEY
        (
            FoodCode,
            VersionID
        )
)
