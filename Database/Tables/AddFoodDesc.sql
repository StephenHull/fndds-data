CREATE TABLE [dbo].[AddFoodDesc]
(
    FoodCode INT,
    SeqNum INT,
    VersionID INT,
    StartDT DATETIME2 NULL,
    EndDT DATETIME2 NULL,
    AdditionalFoodDescription VARCHAR(80) NOT NULL,
    CreateDT DATETIME2
        DEFAULT GETUTCDATE() NOT NULL,
    CONSTRAINT PK_AddFoodDesc
        PRIMARY KEY
        (
            FoodCode,
            SeqNum,
            VersionID
        )
)
