USE Master
GO

--CREATE LOGIN
If NOT EXISTS (SELECT loginname FROM master.dbo.syslogins WHERE name = 'IIS APPPOOL\certifiedoverheadcrane')
BEGIN
	CREATE LOGIN [IIS APPPOOL\certifiedoverheadcrane] FROM WINDOWS WITH DEFAULT_DATABASE=[master], DEFAULT_LANGUAGE=[us_english];
END

USE CertifiedOverheadCrane
GO

--CREATE USER 
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'CertifiedOverheadCrane')
BEGIN
	CREATE USER [CertifiedOverheadCrane] FOR LOGIN [IIS APPPOOL\certifiedoverheadcrane] WITH DEFAULT_SCHEMA=[dbo]
END
GO

EXEC sp_addrolemember 'db_ddladmin', [CertifiedOverheadCrane]

EXEC sp_addrolemember 'db_securityadmin', [CertifiedOverheadCrane]

EXEC sp_addrolemember 'db_datareader', [CertifiedOverheadCrane]

EXEC sp_addrolemember 'db_datawriter', [CertifiedOverheadCrane]