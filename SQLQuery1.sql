-- ============================================================
-- DATABASE: RecyclableDB_Colinares
-- Change "Colinares" to your actual surname everywhere
-- ============================================================

USE master;
GO

IF EXISTS (SELECT name FROM sys.databases WHERE name = 'RecyclableDB_Colinares')
BEGIN
    ALTER DATABASE RecyclableDB_Colinares SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE RecyclableDB_Colinares;
END
GO

CREATE DATABASE RecyclableDB_Colinares;
GO

USE RecyclableDB_Colinares;
GO

-- TABLE: RecyclableType
CREATE TABLE RecyclableType (
    Id      INT IDENTITY(1,1) PRIMARY KEY,
    Type    VARCHAR(100) NOT NULL UNIQUE,
    Rate    DECIMAL(18,2) NOT NULL,
    MinKg   DECIMAL(18,2) NOT NULL,
    MaxKg   DECIMAL(18,2) NOT NULL,
    CONSTRAINT CK_Type_Range CHECK (MinKg < MaxKg),
    CONSTRAINT CK_Type_Rate  CHECK (Rate >= 0)
);
GO

-- TABLE: RecyclableItem
-- ComputedRate calculated via stored procedure, stored as plain column
CREATE TABLE RecyclableItem (
    Id                INT IDENTITY(1,1) PRIMARY KEY,
    RecyclableTypeId  INT NOT NULL,
    ItemDescription   VARCHAR(150) NOT NULL,
    Weight            DECIMAL(18,2) NOT NULL,
    ComputedRate      DECIMAL(18,2) NOT NULL DEFAULT 0,
    CONSTRAINT FK_Item_Type FOREIGN KEY (RecyclableTypeId)
        REFERENCES RecyclableType(Id) ON DELETE CASCADE,
    CONSTRAINT CK_Item_Weight CHECK (Weight > 0)
);
GO

-- SEED DATA
INSERT INTO RecyclableType (Type, Rate, MinKg, MaxKg) VALUES
    ('Plastic',     12.50, 0.5,  50.0),
    ('Metal',       25.75, 1.0, 100.0),
    ('Paper',        8.00, 0.2,  30.0),
    ('Glass',       15.00, 0.5,  75.0),
    ('Electronics', 45.00, 0.1,  20.0);
GO

INSERT INTO RecyclableItem (RecyclableTypeId, ItemDescription, Weight, ComputedRate) VALUES
    (1, 'PET Bottles',     5.50, ROUND(5.50  * 12.50, 2)),
    (2, 'Aluminum Cans',  12.00, ROUND(12.00 * 25.75, 2)),
    (3, 'Cardboard Boxes', 8.25, ROUND(8.25  *  8.00, 2)),
    (1, 'HDPE Containers', 3.75, ROUND(3.75  * 12.50, 2)),
    (5, 'Old Laptops',     2.10, ROUND(2.10  * 45.00, 2));
GO