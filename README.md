# About the project
This repo is about creating a Web Application for an online auctions site as a project for Internet Technologies Course of UoA - DIT.

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
    Execute the EbayAPI/db.sql

## Start UI (as https)
    ng serve --ssl

