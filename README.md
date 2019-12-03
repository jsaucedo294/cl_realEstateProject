# Real Estate Project (CIT 295)
## What is it

Website that allows users to view property deals and calculate what are the best investment options depending on certains filters and preferences.

## Technical Summary

* C#
* Entity Framework for Database
* ASP.NET MVC for front end
* Bootstrap for UI

## How to make project work
* Open Repository on browser https://github.com/jsaucedo294/cl_realEstateProject
* Open Project in Visual Studio
* Go to RealEstatePropertyShared > Data > API > APIKeysTemplate
  * Rename "APIKeysTemplate" class to "APIKeys"
  * Request keys from me or generate your own (AttomKey -> https://api.developer.attomdata.com/signup & ZillowKey -> https://www.zillow.com/howto/api/APIOverview.htm)
  * Add given AttomKey & ZillowKey to APIKey class
* Press F5 to run project
* Open localhost page
* Type zipcode to search for properties for sale

## Features

* Current real estate data
  * Property Details
  * Schools near by
  * Business near by
  * Sales History
  * Persist data in database

* Calculators
  * Property Rental
   - Calculate CAP Rate (Capitalization Rate)
   - Calculate NOI (Net Operating Income)
   - Calculate COC (Cash On Cash return)
  
* Reports
  * Page to view report
  * Pdf report of the property deal ++

* Page to see property deals
  * Ability to filter between deals
  * Ability to add a property ++

## Milestone List

1. Fetch data from Real Estate API (Week 4)

2. Create database to persist property details. (Week 5)
 * MLS Number
 * Street
 * Zip Code
 * House Price
 * Bedrooms
 * Baths
 * Property Square Feet
 * Repairs Needed

3. Create calculators to find best deals (Week 6)

4. Create page to view property deals (Week 7)
