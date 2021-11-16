
projektet GarageMaker är separat program och har inte specifikt med uppgiften att göra


FÖR ATT KÖRA

Hämta NewtonSoft.json v 13.0.1 med package manager. 
Tools -> NuGet Package Manager -> Manage NuGet Packages Manager for Solution -> "Newtonsoft.json" -> välj 13.0.1 -> Bocka "Project" -> Install

Kör programmet Prague Parking 2_0
I /parks finns ett existerande garage som heter "Test" som är strukturerat enligt kraven.

Eller ladda in ett nytt från template genom att välja "Load from GarageMaker/templates" -> "Test"

----------------------------------------------------------------------------------------------------

INSTÄLLNINGAR

I _settings/settings.json går det att ändra priser och andra inställningar. 

Det går att ändra priser samtidigt som programmet körs.

För att ändra storlekar krävs omstart.

----------------------------------------------------------------------------------------------------

SKAPA ETT GARAGE

Man kan också köra programmet GarageMaker för att skapa en egen parkering/p-hus. 
Det sparas som en json i GarageMaker/templates/. 
Den kan sedan laddas in ifrån Prague Parking 2_0 genom att vid uppstart välja att "Load from GarageMaker/templates"
Sen får du välja ett namn på parkeringen som sen sparas i Prague Parking 2_0/parks enligt mallen och fungerar på samma sätt som "Test" garaget som redan finns där.