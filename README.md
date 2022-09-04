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
* Το αρχείο EbayAPI/Dump20220904.sql περιλαμβάνει μία έτοιμη βάση 
δεδομένων με 2500 items (xmls από 0 έως 4) και έτοιμο υπολογισμένο τα 
factorized matrix latents για τα recommendations για αυτά τα items/bids/users.
Το sql αρχείο εκτελείται σε περίπου 2 δευτερόλεπτα, ενώ η κατασκευή από τα xml και ο υπολογισμός
απαιτούν περίπου 5 λεπτά για το κάθε xml και περίπου 20 λεπτά για τον υπολογισμό
2500 αντικειμένων. Υπάρχει πρόβλημα με τις ήδη αποθηκευμένες φωτογραφίες στον πίνακα
Images, οι οποίες δεν εμφανίζονται στο UI, να δώσουμε βάση χωρίς φωτογραφίες και αν
είναι ας προστεθούν μετά.
 



# Known Issues
* Τα services του UI έχουν hardcoded το καθένα το url του api. Να υπάρχει ένα κοινό, για εύκολη αλλαγή θύρας.

# TODO
* Προβολή αντικειμένου από τον seller με όλη τη πληροφορία (βασικά + bids).