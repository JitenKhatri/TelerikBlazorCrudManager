--Create PROCEDURE Get_Users  
--(
DECLARE
    --@Filters FilterTableType READONLY,  
    @PageIndex BIGINT = 1,
    @PageSize BIGINT = 10,
    @SortBy NVARCHAR(40) = N'UserId',
    @SortOrder NVARCHAR(20) = N'Descending';
--)  
--AS  
--BEGIN  
SET NOCOUNT ON;

DECLARE @SQL NVARCHAR(MAX);
DECLARE @Offset BIGINT = (@PageIndex - 1) * @PageSize;


SET @SortBy = ISNULL(@SortBy, 'UserId');
SET @SortOrder = ISNULL(@SortOrder, 'Descending');

-- Start the query  
SET @SQL = N'SELECT *, COUNT(*) OVER() AS TotalCount FROM [User]';

-- Handle filters dynamically  
--IF EXISTS (SELECT 1 FROM @Filters)  
--BEGIN  
DECLARE @WhereClause NVARCHAR(MAX) = N' WHERE 1=1 AND (';

-- Temporary table to store unique field names  
DECLARE @TempFilters TABLE
(
    RowID INT IDENTITY(1, 1),
    Field NVARCHAR(250),
    Operator NVARCHAR(50),
    Value NVARCHAR(250),
    LogicalOperator NVARCHAR(10)
);

-- Insert filters into temporary table  
INSERT INTO @TempFilters
--SELECT Field, Operator, Value, LogicalOperator FROM @Filters;  
VALUES
('LastName', 'Contains', 'King', 'AND');

INSERT INTO @TempFilters
VALUES
('Role', 'IsEqualTo', 'Admin', 'OR');

INSERT INTO @TempFilters
VALUES
('Role', 'IsEqualTo', 'Customer', 'OR');

-- Loop through each field  
DECLARE @Field NVARCHAR(250);
DECLARE @Operator NVARCHAR(50);
DECLARE @Value NVARCHAR(250);
DECLARE @LogicalOperator NVARCHAR(10);
DECLARE @RowCount INT;
DECLARE @RowIndex INT = 1;

SELECT @RowCount = COUNT(*)
FROM @TempFilters;

WHILE @RowIndex <= @RowCount
BEGIN
    SELECT @Field = Field,
           @Operator = Operator,
           @Value = Value,
           @LogicalOperator = LogicalOperator
    FROM @TempFilters
    WHERE RowID = @RowIndex;

    IF @Operator = 'Contains'
    BEGIN
        SET @WhereClause += CASE
                                WHEN @RowIndex > 1 THEN
                                    ' ' + @LogicalOperator
                                ELSE
                                    ''
                            END + N' ' + @Field + N' LIKE ''%' + @Value + N'%''';
    END;
    ELSE IF @Operator = 'IsEqualTo'
    BEGIN
        SET @WhereClause += CASE
                                WHEN @RowIndex > 1 THEN
                                    ' ' + @LogicalOperator
                                ELSE
                                    ''
                            END + N' ' + @Field + N' = ''' + @Value + N'''';
    END;
    ELSE IF @Operator = 'IsNotEqualTo'
    BEGIN
        SET @WhereClause += CASE
                                WHEN @RowIndex > 1 THEN
                                    ' ' + @LogicalOperator
                                ELSE
                                    ''
                            END + N' ' + @Field + N' <> ''' + @Value + N'''';
    END;
    ELSE IF @Operator = 'StartsWith'
    BEGIN
        SET @WhereClause += CASE
                                WHEN @RowIndex > 1 THEN
                                    ' ' + @LogicalOperator
                                ELSE
                                    ''
                            END + N' ' + @Field + N' LIKE ''' + @Value + N'%''';
    END;
    ELSE IF @Operator = 'EndsWith'
    BEGIN
        SET @WhereClause += CASE
                                WHEN @RowIndex > 1 THEN
                                    ' ' + @LogicalOperator
                                ELSE
                                    ''
                            END + N' ' + @Field + N' LIKE ''%' + @Value + N'''';
    END;
    ELSE IF @Operator = 'DoesNotContain'
    BEGIN
        SET @WhereClause += CASE
                                WHEN @RowIndex > 1 THEN
                                    ' ' + @LogicalOperator
                                ELSE
                                    ''
                            END + N' ' + @Field + N' NOT LIKE ''%' + @Value + N'%''';
    END;
    ELSE IF @Operator = 'IsNull'
            OR @Operator = 'IsNullOrEmpty'
    BEGIN
        SET @WhereClause += CASE
                                WHEN @RowIndex > 1 THEN
                                    ' ' + @LogicalOperator
                                ELSE
                                    ''
                            END + N' ' + @Field + N' IS NULL';
    END;
    ELSE IF @Operator = 'IsNotNull'
            OR @Operator = 'IsNotNullOrEmpty'
    BEGIN
        SET @WhereClause += CASE
                                WHEN @RowIndex > 1 THEN
                                    ' ' + @LogicalOperator
                                ELSE
                                    ''
                            END + N' ' + @Field + N' IS NOT NULL';
    END;
    ELSE IF @Operator = 'IsEmpty'
    BEGIN
        SET @WhereClause += CASE
                                WHEN @RowIndex > 1 THEN
                                    ' ' + @LogicalOperator
                                ELSE
                                    ''
                            END + N' ' + @Field + N' = ''''';
    END;
    ELSE IF @Operator = 'IsNotEmpty'
    BEGIN
        SET @WhereClause += CASE
                                WHEN @RowIndex > 1 THEN
                                    ' ' + @LogicalOperator
                                ELSE
                                    ''
                            END + N' ' + @Field + N' != ''''';
    END;
    ELSE IF @Operator = 'IsLessThan'
    BEGIN
        SET @WhereClause += CASE
                                WHEN @RowIndex > 1 THEN
                                    ' ' + @LogicalOperator
                                ELSE
                                    ''
                            END + N' ' + @Field + N' < ' + @Value;
    END;
    ELSE IF @Operator = 'IsLessThanOrEqualTo'
            AND @Field != 'DateOfBirth'
    BEGIN
        SET @WhereClause += CASE
                                WHEN @RowIndex > 1 THEN
                                    ' ' + @LogicalOperator
                                ELSE
                                    ''
                            END + N' ' + @Field + N' <= ' + @Value;
    END;
    ELSE IF @Operator = 'IsLessThanOrEqualTo'
            AND @Field = 'DateOfBirth'
    BEGIN
        SET @WhereClause += CASE
                                WHEN @RowIndex > 1 THEN
                                    ' ' + @LogicalOperator
                                ELSE
                                    ''
                            END + N' ' + @Field + N' <= ''' + @Value + N'''';
    END;
    ELSE IF @Operator = 'IsGreaterThan'
    BEGIN
        SET @WhereClause += CASE
                                WHEN @RowIndex > 1 THEN
                                    ' ' + @LogicalOperator
                                ELSE
                                    ''
                            END + N' ' + @Field + N' > ' + @Value;
    END;
    ELSE IF @Operator = 'IsGreaterThanOrEqualTo'
            AND @Field != 'DateOfBirth'
    BEGIN
        SET @WhereClause += CASE
                                WHEN @RowIndex > 1 THEN
                                    ' ' + @LogicalOperator
                                ELSE
                                    ''
                            END + N' ' + @Field + N' >= ' + CAST(@Value AS NVARCHAR);
    END;
    ELSE IF @Operator = 'IsGreaterThanOrEqualTo'
            AND @Field = 'DateOfBirth'
    BEGIN
        SET @WhereClause += CASE
                                WHEN @RowIndex > 1 THEN
                                    ' ' + @LogicalOperator
                                ELSE
                                    ''
                            END + N' ' + @Field + N' >= ''' + @Value + N'''';
    END;
    SET @RowIndex += 1;
END;

SET @SQL = @SQL + @WhereClause + N')';
--END;  

-- Add ORDER BY and OFFSET-FETCH for pagination  
IF @SortOrder = 'Ascending'
BEGIN
    SET @SQL = @SQL + N' ORDER BY ' + @SortBy + N' ASC';
END;
ELSE
BEGIN
    SET @SQL = @SQL + N' ORDER BY ' + @SortBy + N' DESC';
END;

SET @SQL
    = @SQL + N' OFFSET ' + CAST(@Offset AS NVARCHAR) + N' ROWS FETCH NEXT ' + CAST(@PageSize AS NVARCHAR)
      + N' ROWS ONLY';

-- Debugging: print the SQL query  
PRINT @SQL;

-- Execute the dynamic SQL  
EXEC sp_executesql @SQL;


SELECT FORMAT(DateOfBirth, 'yyyy-MM-dd')
FROM [User];