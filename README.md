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





# Known Issues

* Στο navbar του welcome page να αλλάζει το login button ανάλογα με την κατάσταση του χρήστη. -- fixed
* Το κανονικό navbar έχει θέμα με το Logout (δεν αφαιρεί αμέσως το εικονίδιο χρήστη) -- fixed
* Ημιτελές welcome page -- fixed
* Το register guard θέλει φτιάξιμο (να μην μπορεί να πάει συνδεδεμένος χρήστης) -- fixed
* Στο register form να αλλάξει το alert με swal -- fixed
* Τα services του UI έχουν hardcoded το καθένα το url του api. Να υπάρχει ένα κοινό, για εύκολη αλλαγή θύρας.
