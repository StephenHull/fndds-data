CREATE TABLE [dbo].[FnddsIngred]
(
    FoodCode INT,
    SeqNum INT,
    IngredientCode INT,
    VersionID INT,
    StartDT DATETIME2 NULL,
    EndDT DATETIME2 NULL,
    IngredientDescription VARCHAR(255) NOT NULL,
    Amount DECIMAL(11, 3) NOT NULL,
    Measure VARCHAR(3) NULL,
    PortionCode INT NULL,
    RetentionCode INT NULL,
    Flag INT NULL,
    IngredientWeight DECIMAL(11, 3) NOT NULL,
    ChangeTypeToSRCode VARCHAR(1) NULL,
    ChangeTypeToWeight VARCHAR(1) NULL,
    ChangeTypeToRetnCode VARCHAR(1) NULL,
    CreateDT DATETIME2
        DEFAULT GETUTCDATE() NOT NULL,
    CONSTRAINT PK_FnddsIngred
        PRIMARY KEY
        (
            FoodCode,
            SeqNum,
            IngredientCode,
            VersionID
        )
)
