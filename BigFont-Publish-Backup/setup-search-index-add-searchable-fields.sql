UPDATE [dbo].[Orchard_Search_SearchSettingsPartRecord] 
SET FilterCulture = 0, SearchedFields = 'product-partnumber, product-modelnumber, author, body, title'
WHERE SearchIndex = 'OpenDashboardIndex'

