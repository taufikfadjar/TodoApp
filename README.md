# Todo App Technical Documentation 

## Table of contents
- [Code documentation](#code-documentation)
- [Function header comments](#functions-header-comments)
- [Product documentation](#product-documentation)
- [Software architecture schemas](#software-architecture-schemas)
   - [Logical architecture](#logical-architecture)
   - [Physical Architecture](#physical-architecture)
- [Database schemas](#database-schemas)
- [Sequence diagram](#sequence-diagram)
- [Technical decision log](#Technical-decision-log)

## Code documentation

### Prerequisites

* Visual studio community 2022
* Docker desktop
  
### Install

* Clone the repo
   ```sh
   git clone https://github.com/taufikfadjar/TodoApp.git
   ```
### Execute program
You can choose execute using visual studio or by using docker

  #### Using docker

  * Open terminal & create a new docker network
   ```sh
   docker network create my-network
   ```
  * Go to root TodoApp folder and open terminal for build web api 
   ```sh
   docker build -t webapi-todo -f ./TodoApp.WebApi/Dockerfile .
   ```
  * Run docker web api for expose a port 5000 on host
   ```sh
   docker run --name webapi-todo -p 5000:80  --network my-network -d  webapi-todo
   ```

  * Run migration sqllite by opening browser than hit api migrations
   ```sh
   http://localhost:5000/api/Migration
   ```
  * After finisih execute web api. Next you can execute blazor server.
    First make a changing configuration for API Endpoint. Open TodoApp.BlazorServer folder and edit appsettings.json. 
   ```sh
   "ApiEndPoint": "http://webapi-todo:80/"
   ```
  * Go to root TodoApp folder and open terminal for build blazor
   ```sh
   docker build -t blazor-todo -f ./TodoApp.BlazorServer/Dockerfile .
   ```
  * Run docker blazor for expose a port 5001 on host
   ```sh
   docker run --name blazor-todo -p 5001:80 --network my-network  -d  blazor-todo
   ```
  * After finish you run on browser for doing register user at http://localhost:5001/

  #### Using visual sudio

  * Open visual studio 2022 and choose TodoApp.sln
  * After visual studio was opening project. You can go to **Tools > Nuget Package Manager > Package Manager Console**
  Go root TodoApp.Model and run script on that terminal for applying sql lite db creation
  ```sh
   cd .\TodoApp.Model
   ```
  ```sh
   dotnet ef --startup-project ../TodoApp.WebApi/ database update
   ```
  * Please check a todos.db created or not in TodoApp.WebApi
  * Changing configuration for API Endpoint. Open TodoApp.BlazorServer folder and edit appsettings.json. 
   ```sh
   "ApiEndPoint": "https://localhost:7132/"
   ```
  * For running two application on visual studio. You can right click on solution. Click properties and select multiple project
    Set action TodoApp.BlazorServer to start and set action TodoApp.WebApi to start then apply



## Usage
-

## Product documentation
- Bussiness requirement
  * User Registration: Users are able to register their user id, password, and name
  * User Login: Users are able to perform authentication using user id and password before maintaining their To-Do activities.
  * To-Do Creation: Users are able to input multiple Subject and Description of their To-Do activities. To-Do items should have activities_no where it is a unique sequential activities number with format “AC-XXXX” stored in the database.
  *  To-Do List: Users are able to see a list of their To-Do activities.
  *  To-Do Marking: Users are able to mark their To-Do activity to Done or Canceled, and reverse status to Unmarked.
  *  To-Do Modification: Users are able to modify Subject and Description of Unmarked activity.
  *  To-Do Deletion: Users are able to delete any Unmarked To-Do activity .
  
- Use cases diagram

![use case](https://github.com/taufikfadjar/TodoApp/blob/main/TodoApp/img/Use%20case.png)
## Software Architecture Schemas
- ### Logical architecture 
   
   ![logical architecture](https://github.com/taufikfadjar/TodoApp/blob/main/TodoApp/img/Logical.png)
## Database schemas
- relation of tables and databases

   ![database](https://github.com/taufikfadjar/TodoApp/blob/main/TodoApp/img/DBdiagram.png)
