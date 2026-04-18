ALTER TABLE [dbo].[FnddsIngredEquiv]
ADD CONSTRAINT [FK_FnddsIngredEquiv_FnddsIngred]
    FOREIGN KEY
    (
        FoodCode,
        SeqNum,
        IngredientCode,
        VersionID
    )
    REFERENCES [FnddsIngred]
    (
        FoodCode,
        SeqNum,
        IngredientCode,
        VersionID
    ) ON DELETE CASCADE
