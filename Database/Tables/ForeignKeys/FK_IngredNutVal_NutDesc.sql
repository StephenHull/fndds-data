ALTER TABLE [dbo].IngredNutVal
ADD CONSTRAINT FK_IngredNutVal_NutDesc
    FOREIGN KEY
    (
        NutrientCode,
        VersionID
    )
    REFERENCES [dbo].NutDesc
    (
        NutrientCode,
        VersionID
    ) ON DELETE CASCADE