-- ============================================================
-- VERIFICATION SCRIPT
-- Run this in SSMS to check everything is set up correctly
-- ============================================================

USE RecyclableDB_Colinares;  -- change Colinares to your surname
GO

-- 1. CHECK TABLES EXIST
PRINT '=== TABLES ==='
SELECT TABLE_NAME 
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_TYPE = 'BASE TABLE';

-- 2. CHECK STORED PROCEDURES EXIST
PRINT '=== STORED PROCEDURES ==='
SELECT NAME 
FROM sys.procedures 
WHERE name LIKE 'sp_Recyclable%'
ORDER BY name;

-- 3. CHECK TABLE COLUMNS
PRINT '=== RecyclableType COLUMNS ==='
SELECT COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'RecyclableType';

PRINT '=== RecyclableItem COLUMNS ==='
SELECT COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'RecyclableItem';

-- 4. CHECK SEED DATA
PRINT '=== RecyclableType DATA ==='
SELECT * FROM RecyclableType;

PRINT '=== RecyclableItem DATA ==='
SELECT * FROM RecyclableItem;

-- 5. QUICK STORED PROCEDURE TEST
PRINT '=== TEST: sp_RecyclableType_GetAll ==='
EXEC sp_RecyclableType_GetAll;

PRINT '=== TEST: sp_RecyclableItem_GetAll ==='
EXEC sp_RecyclableItem_GetAll;

PRINT '=== ALL CHECKS DONE ==='