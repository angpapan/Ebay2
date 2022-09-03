# How to execute

## Init UI
    cd EbayUI
    npm i

## Build EbayAPI
Run project from Rider/VS or run:

    dotnet build

## Create db 
(check credentials in EbayAPI/appsettings.json)  
    
    dotnet ef migrations add Init
    dotnet ef database update

## Start UI (as https)
    ng serve --ssl



# Notes
* Στο import-xml τα items εισάγονται με ends year το 2022 ώστε να είναι ενεργά.
* 



# Known Issues
* Τα services του UI έχουν hardcoded το καθένα το url του api. Να υπάρχει ένα κοινό, για εύκολη αλλαγή θύρας.

# TODO
* Προβολή αντικειμένου από τον seller με όλη τη πληροφορία (βασικά + bids).