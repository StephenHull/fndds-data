ALTER TABLE [dbo].FnddsNutVal
ADD CONSTRAINT FK_FnddsNutVal_NutDesc
    FOREIGN KEY
    (
        NutrientCode,
        VersionID
    )
    REFERENCES [dbo].NutDesc
    (
        NutrientCode,
        VersionID
    )