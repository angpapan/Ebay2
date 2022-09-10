# How to execute

## Init UI
    cd EbayUI
    npm i

## Build EbayAPI
Run project from Rider/VS or run:

    dotnet build

## Create db
### By Migrating from models
(check credentials in EbayAPI/appsettings.json)

    dotnet ef migrations add Init
    dotnet ef database update

### By sql script
    Drop database if exists
    Execute the EbayAPI/allEntries.sql

## Start UI (as https)
    ng serve --ssl



# Notes
* Στο import-xml τα items εισάγονται με ends year το 2022 ώστε να είναι ενεργά.
* Το αρχείο EbayAPI/allEntries.sql περιλαμβάνει μία έτοιμη βάση
  δεδομένων με όλα τα items και τις σχέσεις μεταξύ τους (users, categories, bids, itemCategories). Υπάρχει πρόβλημα με τις ήδη αποθηκευμένες φωτογραφίες στον πίνακα
  Images, οι οποίες δεν εμφανίζονται στο UI, να δώσουμε βάση χωρίς φωτογραφίες και αν
  είναι ας προστεθούν μετά.
* Οι πίναες-μοντέλα items, users, categories, bids, itemCategories δεν πρέπει να αλλαχθούν ώστε να λειτουργεί το script.
  Αν αλλάξουν άλλοι πίνακες να τροποποιηθεί και το script κατάλληλα.




# Known Issues
* Τα services του UI έχουν hardcoded το καθένα το url του api. Να υπάρχει ένα κοινό, για εύκολη αλλαγή πόρτας.