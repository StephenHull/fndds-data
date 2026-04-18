CREATE TABLE [dbo].[FnddsIngredEquiv]
(
    FoodCode INT,
    SeqNum INT,
    IngredientCode INT,
    VersionID INT,
    CreateDT DATETIME2
        DEFAULT GETUTCDATE() NOT NULL,
    CONSTRAINT PK_FnddsIngredEquiv
        PRIMARY KEY
        (
            FoodCode,
            SeqNum,
            IngredientCode,
            VersionID
        )
)
