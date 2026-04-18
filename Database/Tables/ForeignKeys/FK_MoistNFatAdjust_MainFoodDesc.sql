ALTER TABLE [dbo].MoistNFatAdjust
ADD CONSTRAINT FK_MoistNFatAdjust_MainFoodDesc
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