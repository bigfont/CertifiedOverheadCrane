USE CertifiedOverheadCrane;
GO

--GET THE DATA THAT WE WANT TO INSERT
DECLARE @FieldData VARCHAR(1000);

SELECT @FieldData = civr.Data
FROM BigFont_OpenDashboard_OpenDashboardContentRecord main
JOIN Title_TitlePartRecord title ON main.Id = title.ContentItemRecord_id
JOIN Orchard_Framework_ContentItemVersionRecord civr ON main.Id = civr.ContentItemRecord_id
WHERE title.Title = 'Template'
AND civr.Latest = 1;

--UPDATE ALL THE SPECIFIED TABLES WITH THAT DATA
UPDATE [dbo].[Orchard_Framework_ContentItemVersionRecord]
SET Data = @FieldData
WHERE ContentItemRecord_id IN 
(
	SELECT Id
	FROM BigFont_OpenDashboard_OpenDashboardContentRecord
)


/*
SELECT main.*, civr.* --, tpr.Title, bpr.Text, cpvr.*
FROM [dbo].[BigFont_OpenDashboard_OpenDashboardContentRecord] main
LEFT JOIN [dbo].[Orchard_Framework_ContentItemVersionRecord] civr ON civr.ContentItemRecord_id = main.id
LEFT JOIN [dbo].[Title_TitlePartRecord] tpr ON tpr.ContentItemRecord_id = main.id
LEFT JOIN [dbo].[Common_BodyPartRecord] bpr ON bpr.ContentItemRecord_id = main.id
LEFT JOIN Common_CommonPartVersionRecord cpvr ON cpvr.ContentItemRecord_id = main.id
LEFT JOIN Orchard_Autoroute_AutoroutePartRecord apr ON apr.ContentItemRecord_id = main.id
*/