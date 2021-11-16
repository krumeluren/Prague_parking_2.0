

FÖR ATT KÖRA
Hämta NewtonSoft.json v 9.0.1 med package manager. 
Tools -> NuGet Package Manager -> Manage NuGet Packages Manager for Solution -> "Newtonsoft.json" -> välj 9.0.1 -> Bocka "Project" -> Install

Kör programmet Pargue Parking 2_0
I /parks finns ett färdigt garage som heter "Test" som är strukturerat enligt specifikationerna.

----------------------------------------------------------------------------------------------------

INSTÄLLNINGAR
I _settings/settings.json går det att ändra priser och andra inställningar. Det går att ändra samtidigt som programmet körs.


----------------------------------------------------------------------------------------------------

SKAPA ETT GARAGE
Man kan också köra programmet GarageMaker för att skapa en egen parkering/p-hus. 
Det sparas som en json i GarageMaker/templates/. 
Den kan sedan laddas in ifrån Prague Parking 2_0 genom att vid uppstart välja att "ladda in ifrån GarageMaker/templates"
Sen får du välja ett namn på parkeringen som sen sparas i Prague Parking 2_0/parks enligt mallen och fungerar på samma sätt som "Test" garaget.