IF NOT EXISTS (SELECT name FROM sys.server_principals WHERE name = 'IIS APPOOL\OnlineKladilnicaAppPool')
BEGIN
    CREATE LOGIN [IIS APPOOL\OnlineKladilnicaAppPool] 
      FROM WINDOWS WITH DEFAULT_DATABASE=[master], 
      DEFAULT_LANGUAGE=[us_english]
END
GO
CREATE USER OnlineKladilnicaUser 
  FOR LOGIN [IIS APPOOL\OnlineKladilnicaAppPool]
GO
EXEC sp_addrolemember 'db_owner', 'OnlineKladilnicaUser'
GO