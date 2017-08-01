CREATE LOGIN [username] WITH password='password';

CREATE USER [usernamem]
    FOR LOGIN [username]
    WITH DEFAULT_SCHEMA = dbo
GO

EXEC sp_addrolemember N'db_datareader', N'username'
GO

EXEC sp_addrolemember N'db_datawriter', N'username'
GO

--ALTER LOGIN [username] WITH 
--     PASSWORD = '' 
--     OLD_PASSWORD = '';
--GO
