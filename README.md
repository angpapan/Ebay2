## Init UI
    cd EbayUI
    npm i

## Build EbayAPI
Run project from Rider/VS

## Create db 
(check credentials in EbayAPI/appsettings.json)  
    
    dotnet ef migrations add Init
    dotnet ef database update

## Start UI (as https)
    ng serve --ssl

