# Customer API

A simple RESTful API in C#/.NET 8 to manage a list of customers. Customers are validated and inserted into an in-memory list in sorted order by last name and first name (without using built-in sorting). The list is persisted to disk between restarts.

---

## Features

- `POST /customers` – Add one or more customers (method name: `Add`)
- `GET /customers` – Retrieve all customers sorted by last name, then first name.
- Validates:
  - Required fields (`FirstName`, `LastName`, `Age`, `Id`)
  - Age must be above 18
  - ID must not be a duplicate
  - If any validation failes, returns:
	```json
	{
	  "errors": [
		"[0].Age: Age must be greater than 18",
		"Customer ID 1 already exists."
	  ]
	}

- Data Store: The API uses a pluggable store via ICustomerStore. You can choose between
	- File: persistent JSON (customer.json) for local use
	- Memory: in-memory store (default for could deployments)
	- appsetting.json
	```json
	{
	  "CustomerStore": {
		"Provider": "File"  // or "Memory"
	  }
	}

---

## Running the API locally (The API will be avaliable at http://localhost:5275)

### Option A: With Visual Studio
1. Open the solution in Visual Studio
2. Set `CustomerApi` as startup project
3. Press **F5** to launch


### Option B: With .NET CLI
``bash
cd CustomerApi
dotnet run

## Running You can delpoy to Azure App Service (Windows) using Visual Studio or GitHub Actions

### The GitHub Actions workflow is located at:
.github/workflows/deploy.yml

### The API currently is available at https://customerapi-service.azurewebsites.net/