USE RecyclableDB_Colinares;
GO

-- ============================================================
-- RECYCLABLE TYPE STORED PROCEDURES
-- ============================================================

CREATE OR ALTER PROCEDURE sp_RecyclableType_GetAll
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Type, Rate, MinKg, MaxKg
    FROM RecyclableType
    ORDER BY Type ASC;
END
GO

CREATE OR ALTER PROCEDURE sp_RecyclableType_GetById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Type, Rate, MinKg, MaxKg
    FROM RecyclableType
    WHERE Id = @Id;
END
GO

CREATE OR ALTER PROCEDURE sp_RecyclableType_Insert
    @Type   VARCHAR(100),
    @Rate   DECIMAL(18,2),
    @MinKg  DECIMAL(18,2),
    @MaxKg  DECIMAL(18,2),
    @NewId  INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    IF @MinKg >= @MaxKg
    BEGIN RAISERROR('MinKg must be less than MaxKg.', 16, 1); RETURN; END
    IF EXISTS (SELECT 1 FROM RecyclableType WHERE Type = @Type)
    BEGIN RAISERROR('Type already exists.', 16, 1); RETURN; END

    INSERT INTO RecyclableType (Type, Rate, MinKg, MaxKg)
    VALUES (@Type, @Rate, @MinKg, @MaxKg);
    SET @NewId = SCOPE_IDENTITY();
END
GO

CREATE OR ALTER PROCEDURE sp_RecyclableType_Update
    @Id     INT,
    @Type   VARCHAR(100),
    @Rate   DECIMAL(18,2),
    @MinKg  DECIMAL(18,2),
    @MaxKg  DECIMAL(18,2)
AS
BEGIN
    SET NOCOUNT ON;
    IF @MinKg >= @MaxKg
    BEGIN RAISERROR('MinKg must be less than MaxKg.', 16, 1); RETURN; END
    IF EXISTS (SELECT 1 FROM RecyclableType WHERE Type = @Type AND Id <> @Id)
    BEGIN RAISERROR('Type name already exists.', 16, 1); RETURN; END

    UPDATE RecyclableType
    SET Type = @Type, Rate = @Rate, MinKg = @MinKg, MaxKg = @MaxKg
    WHERE Id = @Id;

    -- Recalculate ComputedRate for all linked items when rate changes
    UPDATE RecyclableItem
    SET ComputedRate = ROUND(Weight * @Rate, 2)
    WHERE RecyclableTypeId = @Id;
END
GO

CREATE OR ALTER PROCEDURE sp_RecyclableType_Delete
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM RecyclableType WHERE Id = @Id;
END
GO

-- ============================================================
-- RECYCLABLE ITEM STORED PROCEDURES
-- ============================================================

CREATE OR ALTER PROCEDURE sp_RecyclableItem_GetAll
AS
BEGIN
    SET NOCOUNT ON;
    SELECT
        ri.Id,
        ri.RecyclableTypeId,
        rt.Type AS RecyclableTypeName,
        ri.ItemDescription,
        ri.Weight,
        ri.ComputedRate
    FROM RecyclableItem ri
    INNER JOIN RecyclableType rt ON ri.RecyclableTypeId = rt.Id
    ORDER BY ri.Id DESC;
END
GO

CREATE OR ALTER PROCEDURE sp_RecyclableItem_GetById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT
        ri.Id,
        ri.RecyclableTypeId,
        rt.Type AS RecyclableTypeName,
        ri.ItemDescription,
        ri.Weight,
        ri.ComputedRate
    FROM RecyclableItem ri
    INNER JOIN RecyclableType rt ON ri.RecyclableTypeId = rt.Id
    WHERE ri.Id = @Id;
END
GO

CREATE OR ALTER PROCEDURE sp_RecyclableItem_Insert
    @RecyclableTypeId  INT,
    @ItemDescription   VARCHAR(150),
    @Weight            DECIMAL(18,2),
    @NewId             INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS (SELECT 1 FROM RecyclableType WHERE Id = @RecyclableTypeId)
    BEGIN RAISERROR('RecyclableType does not exist.', 16, 1); RETURN; END
    IF @Weight <= 0
    BEGIN RAISERROR('Weight must be greater than 0.', 16, 1); RETURN; END

    DECLARE @Rate DECIMAL(18,2);
    SELECT @Rate = Rate FROM RecyclableType WHERE Id = @RecyclableTypeId;

    INSERT INTO RecyclableItem (RecyclableTypeId, ItemDescription, Weight, ComputedRate)
    VALUES (@RecyclableTypeId, @ItemDescription, @Weight, ROUND(@Weight * @Rate, 2));

    SET @NewId = SCOPE_IDENTITY();
END
GO

CREATE OR ALTER PROCEDURE sp_RecyclableItem_Update
    @Id                INT,
    @RecyclableTypeId  INT,
    @ItemDescription   VARCHAR(150),
    @Weight            DECIMAL(18,2)
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS (SELECT 1 FROM RecyclableType WHERE Id = @RecyclableTypeId)
    BEGIN RAISERROR('RecyclableType does not exist.', 16, 1); RETURN; END
    IF @Weight <= 0
    BEGIN RAISERROR('Weight must be greater than 0.', 16, 1); RETURN; END

    DECLARE @Rate DECIMAL(18,2);
    SELECT @Rate = Rate FROM RecyclableType WHERE Id = @RecyclableTypeId;

    UPDATE RecyclableItem
    SET RecyclableTypeId = @RecyclableTypeId,
        ItemDescription  = @ItemDescription,
        Weight           = @Weight,
        ComputedRate     = ROUND(@Weight * @Rate, 2)
    WHERE Id = @Id;
END
GO

CREATE OR ALTER PROCEDURE sp_RecyclableItem_Delete
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM RecyclableItem WHERE Id = @Id;
END
GO