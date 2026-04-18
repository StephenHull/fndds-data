ALTER TABLE [dbo].ModDesc
ADD CONSTRAINT FK_ModDesc_MainFoodDesc
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
