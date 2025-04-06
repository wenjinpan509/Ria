
---

# Customer Simulator

A parallel test client for the Customer API. It sends random POST and GET requests to simulate multiple clients adding and retrieving customers concurrently.

---

## Features

- Sends multiple `POST` requests with:
  - 2–3 random customers each
  - Random first/last name combinations
  - Random age (10–90)
  - Auto-incrementing IDs
- Sends multiple `GET` requests to retrieve customer list
- Runs all requests in parallel

---

## Configuration

### `appsettings.json`

```json
{
  "Api": {
    "BaseAddress": "https://customerapi-service.azurewebsites.net/"
  }
}

change this to local or deployed API address:
```json
{
  "Api": {
    "BaseAddress": "http://localhost:5275/"
  }
}