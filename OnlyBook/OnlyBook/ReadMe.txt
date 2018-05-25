Project Name - LibrarySystem

Things you will want to look at:
	- Models/Book.cs - This is how I represent all of my data.  

	- Repository - Where we actually get there data.  This is  
						setup for Dapper

	- Controllers/* - This is how all the requests get to the REpository.  

    - DDL.txt - This is the script to create the sctucture in Postgres


Tools I use:
	- Visual Studio 2017 - The IDE
	- Datagrip - DB interface tool - https://www.jetbrains.com/datagrip/

Libraries (already installed in this package)
	- Dapper
	- Npgsql
	- Swagger

Functionalaties covered:

- Basic CRUD for Book, Patron, Library, Author
- Patron can checkout/return a book
- We can search author by its name
- We can add book to an author (many to many)