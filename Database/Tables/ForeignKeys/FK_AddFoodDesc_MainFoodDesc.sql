ALTER TABLE [dbo].AddFoodDesc
ADD CONSTRAINT FK_AddFoodDesc_MainFoodDesc
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