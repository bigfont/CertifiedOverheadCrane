USE CertifiedOverheadCrane
GO

/****** Object:  User [GardenFaire]    Script Date: 5/28/2013 1:12:16 AM ******/
CREATE USER [CertifiedOverheadCrane] FOR LOGIN [IIS APPPOOL\certifiedoverheadcrane] WITH DEFAULT_SCHEMA=[dbo]
GO

EXEC sp_addrolemember 'db_ddladmin', [CertifiedOverheadCrane]

EXEC sp_addrolemember 'db_securityadmin', [CertifiedOverheadCrane]

EXEC sp_addrolemember 'db_datareader', [CertifiedOverheadCrane]

EXEC sp_addrolemember 'db_datawriter', [CertifiedOverheadCrane]