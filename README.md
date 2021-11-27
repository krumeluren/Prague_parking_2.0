Projektet 'GarageMaker' är separat program och har inte specifikt med uppgiften att göra


FÖR ATT KÖRA

Om inte installerat, hämta NewtonSoft.json v 13.0.1 eller 9.0.1 med package manager. 
Tools -> NuGet Package Manager -> Manage NuGet Packages Manager for Solution -> "Newtonsoft.json" -> välj v 13.0.1 eller 9.0.1 -> Bocka "Project" -> Install - Rebuild project

Kör programmet Prague Parking 2_0

I /parks finns ett existerande garage som heter "Test" som är strukturerat enligt specifikationerna.

Eller ladda in ett nytt från template genom att välja "Load from GarageMaker/templates" -> "Test"

För att skapa ett template läs "SKAPA ETT GARAGE"

----------------------------------------------------------------------------------------------------

INSTÄLLNINGAR

I _settings/settings.json går det att ändra priser och andra inställningar. 

Det går att ändra priser samtidigt som programmet körs.

För att ändra storlekar krävs omstart av programmet för att det ska ändras.

----------------------------------------------------------------------------------------------------

SKAPA ETT GARAGE

Man kan också köra programmet GarageMaker för att skapa en egen parkering/p-hus. 
Det sparas som en json i GarageMaker/templates/. 
Den kan sedan laddas in ifrån Prague Parking 2_0 genom att vid uppstart välja att "Load from GarageMaker/templates"
Sen får du välja ett namn på parkeringen som sen sparas i Prague Parking 2_0/parks enligt mallen och fungerar på samma sätt som "Test" garaget som redan finns där.
