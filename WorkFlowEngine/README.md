# QUICK START GUIDE

---

````markdown
# Workflow Engine – Infonetica Task

A minimal backend service in .NET 8 / C# to manage configurable workflow state machines.

---

## Features

- Define workflows (states + actions)
- Start workflow instances
- Execute actions with full validation
- Query workflows, states, actions, and instances

---

## Requirements

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Postman](https://www.postman.com/downloads/) (for testing)

---

## Running Locally (Windows)

1. Open terminal (PowerShell or CMD)
2. Navigate to project root: /cd WorkflowEngine
````

3. Run the app:

   ```bash
   dotnet run
   ```

4. Service runs at:

   ```
   http://localhost:5000
   ```

---

## Postman Testing Guide

### 1. Create Workflow Definition

* **POST** `http://localhost:5000/workflows`
* **Body (raw, JSON):**

```json
{
  "states": [
    { "id": "start", "name": "Start", "isInitial": true },
    { "id": "review", "name": "Review" },
    { "id": "done", "name": "Done", "isFinal": true }
  ],
  "actions": [
    { "id": "toReview", "name": "To Review", "fromStates": ["start"], "toState": "review" },
    { "id": "complete", "name": "Complete", "fromStates": ["review"], "toState": "done" }
  ]
}
```

---

### 2. Start Workflow Instance

* **POST** `http://localhost:5000/instances`
* **Body:**

```json
{
  "workflowId": "<copy-from-previous-response>"
}
```

---

### 3. Execute Actions

* **To move from `start` → `review`:**

  ```http
  POST /instances/{instanceId}/actions/toReview
  ```

* **Then from `review` → `done`:**

  ```http
  POST /instances/{instanceId}/actions/complete
  ```

---

### 4. Check Instance State & History

* **GET** `http://localhost:5000/instances/{instanceId}`

---

## Example Workflow IDs

The app generates random GUIDs like:

```
82921a6b-b0b2-48e5-ae30-6366cf971fa2
```

These are safe to share and can be used directly in testing.

---

## Notes

* Data is **stored in-memory**; restarting the app resets everything.
* Minimal error messages are returned for invalid transitions or states.
* Designed using ASP.NET Core **Minimal API** and **clean service structure**.

---

## Endpoints Overview

| Method | Endpoint                             | Purpose                       |
| ------ | ------------------------------------ | ----------------------------- |
| POST   | `/workflows`                         | Create a new workflow         |
| GET    | `/workflows`                         | List all workflow definitions |
| GET    | `/workflows/{id}`                    | Get a workflow by ID          |
| POST   | `/instances`                         | Start a new instance          |
| GET    | `/instances`                         | List all instances            |
| GET    | `/instances/{id}`                    | Get instance state + history  |
| POST   | `/instances/{id}/actions/{actionId}` | Execute an action transition  |

---

## Project Structure

```
WorkflowEngine/
├── Models/         # State, Action, WorkflowDefinition, WorkflowInstance
├── Services/       # Business logic
├── Program.cs      # API endpoints
├── WorkflowEngine.csproj
└── README.md
```

---

## Submission Notes

* No database used
* No extra dependencies added
* All logic is self-contained

---

## Results
Please refer to folder /results in main