ALTER TABLE [dbo].[FnddsIngredEquiv]
ADD CONSTRAINT [FK_FnddsIngredEquiv_IngredEquiv]
    FOREIGN KEY
    (
        IngredientCode,
        VersionID
    )
    REFERENCES [IngredEquiv]
    (
        IngredientCode,
        VersionID
    ) ON DELETE CASCADE
