CREATE TABLE [dbo].[FnddsNutVal]
(
    FoodCode INT,
    NutrientCode INT,
    VersionID INT,
    StartDT DATETIME2 NULL,
    EndDT DATETIME2 NULL,
    NutrientValue DECIMAL(10, 3) NOT NULL,
    CreateDT DATETIME2
        DEFAULT GETUTCDATE() NOT NULL,
    CONSTRAINT PK_FnddsNutVal
        PRIMARY KEY
        (
            FoodCode,
            NutrientCode,
            VersionID
        )
)
