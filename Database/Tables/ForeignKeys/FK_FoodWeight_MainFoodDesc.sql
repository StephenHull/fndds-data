ALTER TABLE [dbo].FoodWeight
ADD CONSTRAINT FK_FoodWeight_MainFoodDesc
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
