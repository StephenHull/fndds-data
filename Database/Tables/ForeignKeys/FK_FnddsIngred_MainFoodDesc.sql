ALTER TABLE [dbo].FnddsIngred
ADD CONSTRAINT FK_FnddsIngred_MainFoodDesc
    FOREIGN KEY
    (
        FoodCode,
        VersionID
    )
    REFERENCES [dbo].MainFoodDesc
    (
        FoodCode,
        VersionID
    ) ON DELETE CASCADE