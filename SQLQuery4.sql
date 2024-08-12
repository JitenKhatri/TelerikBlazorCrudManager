--CREATE TABLE Country (
--    CountryId BIGINT PRIMARY KEY,        -- Primary key column with BIGINT data type
--    CountryName NVARCHAR(MAX),           -- Column for the country name with NVARCHAR(MAX) data type
--    ISO NVARCHAR(10),                    -- Column for the ISO code with NVARCHAR(10) data type (adjust length as needed)
--    CreatedAt DATETIME,                  -- Column for the creation timestamp
--    UpdatedAt DATETIME,                  -- Column for the last updated timestamp
--    DeletedAt DATETIME                   -- Column for the deletion timestamp (can be NULL if not deleted)
--);


--CREATE TABLE City (
--    CityId BIGINT PRIMARY KEY,           -- Primary key column with BIGINT data type
--    CityName NVARCHAR(MAX),              -- Column for the city name with NVARCHAR(MAX) data type
--    CountryId BIGINT,                    -- Column for the foreign key reference to the Country table
--    CreatedAt DATETIME,                 -- Column for the creation timestamp
--    UpdatedAt DATETIME,                 -- Column for the last updated timestamp
--    DeletedAt DATETIME,                 -- Column for the deletion timestamp (can be NULL if not deleted)
    
--    -- Foreign key constraint to ensure City.CountryId matches a valid CountryId in the Country table
--    FOREIGN KEY (CountryId) REFERENCES Country(CountryId)
--);

--CREATE TABLE [User] (
--    UserId BIGINT PRIMARY KEY,                   -- Primary key column with BIGINT data type
--    UserName NVARCHAR(255) NOT NULL,            -- Column for the username with a maximum length of 255 characters
--    Password NVARCHAR(255) NOT NULL,            -- Column for the password with a maximum length of 255 characters
--    FirstName NVARCHAR(255) NOT NULL,           -- Column for the first name with a maximum length of 255 characters
--    LastName NVARCHAR(255) NOT NULL,            -- Column for the last name with a maximum length of 255 characters
--    DateOfBirth DATE,                           -- Column for the date of birth
--    PhoneNumber NVARCHAR(20),                   -- Column for the phone number with a maximum length of 20 characters
--    Role NVARCHAR(50) CHECK (Role IN ('Admin', 'User')), -- Column for user role with a constraint to ensure values are either 'Admin' or 'User'
--    CountryId BIGINT,                           -- Column for the foreign key reference to the Country table
--    CityId BIGINT,                              -- Column for the foreign key reference to the City table
--    CreatedAt DATETIME DEFAULT GETDATE(),       -- Column for the creation timestamp with a default value of the current date and time
--    UpdatedAt DATETIME DEFAULT GETDATE(),       -- Column for the last updated timestamp with a default value of the current date and time
--    DeletedAt DATETIME,                        -- Column for the deletion timestamp (can be NULL if not deleted)
    
--    -- Foreign key constraint to ensure User.CountryId matches a valid CountryId in the Country table
--    FOREIGN KEY (CountryId) REFERENCES Country(CountryId),
    
--    -- Foreign key constraint to ensure User.CityId matches a valid CityId in the City table
--    FOREIGN KEY (CityId) REFERENCES City(CityId)
--);


--INSERT INTO Country (CountryId, CountryName, ISO, CreatedAt, UpdatedAt, DeletedAt)
--VALUES
--(1, 'United States', 'US', GETDATE(), GETDATE(), NULL),
--(2, 'Canada', 'CA', GETDATE(), GETDATE(), NULL),
--(3, 'Mexico', 'MX', GETDATE(), GETDATE(), NULL),
--(4, 'United Kingdom', 'GB', GETDATE(), GETDATE(), NULL),
--(5, 'Germany', 'DE', GETDATE(), GETDATE(), NULL),
--(6, 'France', 'FR', GETDATE(), GETDATE(), NULL),
--(7, 'Italy', 'IT', GETDATE(), GETDATE(), NULL),
--(8, 'Spain', 'ES', GETDATE(), GETDATE(), NULL),
--(9, 'Australia', 'AU', GETDATE(), GETDATE(), NULL),
--(10, 'Japan', 'JP', GETDATE(), GETDATE(), NULL);


--INSERT INTO City (CityId, CityName, CountryId, CreatedAt, UpdatedAt, DeletedAt)
--VALUES
--(1, 'New York', 1, GETDATE(), GETDATE(), NULL),
--(2, 'Los Angeles', 1, GETDATE(), GETDATE(), NULL),
--(3, 'Toronto', 2, GETDATE(), GETDATE(), NULL),
--(4, 'Vancouver', 2, GETDATE(), GETDATE(), NULL),
--(5, 'Mexico City', 3, GETDATE(), GETDATE(), NULL),
--(6, 'London', 4, GETDATE(), GETDATE(), NULL),
--(7, 'Berlin', 5, GETDATE(), GETDATE(), NULL),
--(8, 'Paris', 6, GETDATE(), GETDATE(), NULL),
--(9, 'Rome', 7, GETDATE(), GETDATE(), NULL),
--(10, 'Madrid', 8, GETDATE(), GETDATE(), NULL);

--CREATE PROCEDURE Register_User
--    @UserName NVARCHAR(255),
--    @Password NVARCHAR(255),
--    @FirstName NVARCHAR(255),
--    @LastName NVARCHAR(255),
--    @DateOfBirth DATE,
--    @PhoneNumber NVARCHAR(20),
--    @Role NVARCHAR(50),
--    @CountryId BIGINT,
--    @CityId BIGINT
--AS
--BEGIN
--    -- Insert a new record into the User table

--	DECLARE @lUserName NVARCHAR(255) = @UserName,
--			@lPassword NVARCHAR(255) = @Password,
--			@lFirstName NVARCHAR(255) = @FirstName,
--			@lLastName NVARCHAR(255) = @LastName,
--			@lDateOfBirth DATE = @DateOfBirth,
--			@lPhoneNumber NVARCHAR(20) = @PhoneNumber,
--			@lRole NVARCHAR(50) = @Role,
--			@lCountryId BIGINT = @CountryId,
--			@lCityId BIGINT = @CityId
--    INSERT INTO [User] (UserName, Password, FirstName, LastName, DateOfBirth, PhoneNumber, Role, CountryId, CityId, CreatedAt, UpdatedAt)
--    VALUES (
--        @lUserName,
--        @lPassword,
--        @lFirstName,
--        @lLastName,
--        @lDateOfBirth,
--        @lPhoneNumber,
--        @lRole,
--        @lCountryId,
--        @lCityId,
--        GETDATE(), -- Set CreatedAt to the current date and time
--        GETDATE()  -- Set UpdatedAt to the current date and time
--    );
--END;

--CREATE PROCEDURE Varify_User
--(
--@UserName nvarchar(250),
--@Password nvarchar(250)
--)
--AS
--BEGIN
--DECLARE @lUserName nvarchar(250) = @UserName,
--@lPassword nvarchar(250) = @Password

--SELECT * FROM [User] where UserName = @lUserName AND Password = @lPassword
--END

--CREATE PROCEDURE Get_Country
--AS 
--BEGIN
--SELECT * FROM Country
--END

--CREATE PROCEDURE Get_Cities_By_Country
--    @CountryId BIGINT
--AS
--BEGIN
--DECLARE @lCountryId BIGINT = @CountryId
--    -- Select cities based on the provided CountryId
--    SELECT
--       *
--    FROM
--        City
--    WHERE
--        CountryId = @lCountryId;
--END;

--CREATE TABLE NavigationItem (
--    Id BIGINT IDENTITY(1,1) PRIMARY KEY,
--    Text NVARCHAR(255) NOT NULL,
--    Url NVARCHAR(255) NOT NULL,
--    Icon NVARCHAR(255),
--    AdminOnly BIT NOT NULL,
--	ParentId BIGINT,
--    HasChildren BIT NOT NULL
--);


--CREATE PROCEDURE GetNavigationItems
--AS
--BEGIN

--SELECT * FROM NavigationItem where ParentId IS NULL

--END


--CREATE PROCEDURE GetChildrenNavigationItems
--(
--@ParentId BIGINT
--)
--AS
--BEGIN 
--DECLARE @lParentId BIGINT = @ParentId;

--SELECT * FROM NavigationItem WHERE ParentId = @lParentId;
--END



--CREATE TYPE dbo.FilterType AS TABLE
--(
--    Field NVARCHAR(MAX),
--    Operator NVARCHAR(MAX),
--    Value NVARCHAR(MAX),
--    LogicalOperator NVARCHAR(MAX)
--);



