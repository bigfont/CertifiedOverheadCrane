---------------------
Development vs Publish Directories
---------------------

"C:\Users\Shaun\Documents\GitHub\CertifiedOverheadCrane\orchard"
-This is the directory in which we are developing modules / themes / recipes.

"C:\Users\Shaun\Documents\GitHub\CertifiedOverheadCrane\orchard1x"
-This is the directory in which we pull the tip of the 1.x branch from Mercurial,
-then we build a deployment package from it, 
-then we follow the steps below. 

---------------------
Before Getting Started
---------------------

Copy / import the contents of your orchard1x deployment package to the site's root directory.
Create an empty App_Data directory or delete the contents of the existing one (dependencies doesn't matter).
Grant Access Control *Modify* permissions for the IIS Apppool for these folders.
	App_Data
	Media
	Themes	
	Modules
Copy modules, themes and recipes from orchard that will not come from the Gallery. (required)
	Modules\Orchard.Setup\Recipes\certifiedOverheadCrane.recipe.xml
	Modules\BigFont.OpenDashboard
	Modules\Contrib.ImageField
	Themes\CertifiedOverheadCrane
Create an empty CertifiedOverheadCrane DB.
Setup DB permissions using IIS-DatabaseUser.sql

---------------------
Get Started
---------------------

Certified Overhead Crane
admin
p@ssw0rd
p@ssw0rd
Existing SQL Server Express DB
data source=FONTY\SQLEXPRESS;initial catalog=CertifiedOverheadCrane;integrated security=True;

Server=tcp:my6wvevkhg.database.windows.net,1433;Database=cranes;User ID=bigfont@my6wvevkhg;Password=673W&7zqyhFt!F;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;

Certified Overhead Crane

---------------------
Select the Theme
---------------------

Admin > Themes > CertifiedOverheadCrane

---------------------
Populate Test Data
---------------------

Follow the instructions within TestData-PopulateFields.sql

---------------------
Setup Search
---------------------

While the recipe sets up most of the search functionality; we need to add Fields and Content Types.
Run SearchIndex-AddSearchableFields.sql
Update AND rebuild the Crane Parts index.

---------------------
Basic Testing
---------------------

Search by all the relevant parts & fields.

---------------------
Components
---------------------

Modules
-BigFont.OpenDashboard
-Contrib.ImageField

Themes
-Bootstrap
-CertifiedOverheadCrane

Recipes
-certifiedOverheadCrane.reciple.xml