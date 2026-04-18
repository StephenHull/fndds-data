CREATE TABLE [dbo].[IngredNutVal]
(
    IngredientCode INT,
    NutrientCode INT,
    VersionID INT,
    StartDT DATETIME2 NULL,
    EndDT DATETIME2 NULL,
    IngredientDescription VARCHAR(200) NOT NULL,
    NutrientValue DECIMAL(10, 3) NOT NULL,
    NutrientValueSource VARCHAR(80) NOT NULL,
    FdcID INT NULL,
    DerivationCode VARCHAR(4) NULL,
    SRAddModYear INT NULL,
    FoundationYearAcquired INT NULL,
    CreateDT DATETIME2
        DEFAULT GETUTCDATE() NOT NULL,
    CONSTRAINT PK_IngredNutVal
        PRIMARY KEY
        (
            IngredientCode,
            NutrientCode,
            VersionID
        )
)
