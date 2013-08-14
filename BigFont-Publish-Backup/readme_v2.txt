
# Notes:

"\GitHub\CertifiedOverheadCrane\orchard"

-This is the directory in which we are developing modules / themes / recipes.

"\GitHub\CertifiedOverheadCrane\orchard1x"

-This is the directory in which we pull the tip of the 1.x branch from Mercurial,
-then we build a deployment package from it, 
-then we follow the steps below. 

# Publish to Windows Azure Website

1. Create an Azure Website

2. Create an Azure SQL Database

3. Run build "compile;package" from orchard1x. (about 5-minutes)

4. FTP contents of orchard1x\build\Stage to the wwwroot of an Azure Website. (about 10-minutes)

5. Copy modules, recipe, and theme from orchard (not orchard1x) that will not come from the Gallery (about 5-minutes)

Modules
- \orchard\src\Orchard.Web\Modules\BigFont.OpenDashboard
- \orchard\src\Orchard.Web\Modules\Contrib.ImageField

Recipe
- \orchard\src\Orchard.Web\Modules\Orchard.Setup\Recipes\certifiedOverheadCrane.recipe.xml

Theme
- currently not working
- skip

6. Navigate to the Windows Azure Website domain (e.g. cranes.azurewebsites.net). Complete Getting Started.

- Certified Overhead Crane
- admin
- p@ssw0rd (save these creds in LastPass)
- p@ssw0rd
- Existing SQL Server Express DB
- Connection string to Azure SQL Database from step (2)
- Certified Overhead Crane (recipe)

Server=tcp:my6wvevkhg.database.windows.net,1433;Database=cranes_db;User ID=bigfont@my6wvevkhg;Password=673W&7zqyhFt!F;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;

7. Dashboard > Themes > The Theme Machine > Set Current

8. Add test data. Follow the instructions within setup-populate-test-data-files.sql

9. Setup search. 

- While the recipe sets up most of the search functionality; we need to add Fields and Content Types.
- Run setup-search-index-add-searchable-fields.sql
- Then, update AND rebuild the Crane Parts index (dashboard > settings caret > indexes >  update & rebuild)

10. Do some basic testing by searching fields etc. 
