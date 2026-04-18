CREATE TABLE [dbo].[NutDesc]
(
    NutrientCode INT,
    VersionID INT,
    NutrientDescription VARCHAR(45) NOT NULL,
    Tagname VARCHAR(15) NULL,
    Unit VARCHAR(10) NOT NULL,
    Decimals INT NOT NULL,
    CreateDT DATETIME2
        DEFAULT GETUTCDATE() NOT NULL,
    CONSTRAINT PK_NutDesc
        PRIMARY KEY
        (
            NutrientCode,
            VersionID
        )

)