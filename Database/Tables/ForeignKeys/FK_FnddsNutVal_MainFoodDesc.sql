ALTER TABLE [dbo].FnddsNutVal
ADD CONSTRAINT FK_FnddsNutVal_MainFoodDesc
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