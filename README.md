# DK.AuthService

## Status
- Email authorization - READY TO USE
- Google authorization - WORK IN PROGRESS
- Apple authorization - WORK IN PROGRESS

## Setup
The application uses **SQL Server** as a database.
- First of all, change the ***Connection String*** in **appsettings.json** to match the database you use.
- Then just launch the app and use Swagger (or some other tool) to interact with it. The database will be created automatically when you run the app the first time.

## Usage
Default Admin credentials are:
- *admin@mail.com*
- *password*

Change them after the first launch.

## Log in to this service using Swagger
- To get a **token for authorization,** use the Login method (the user must exist in the system. Or you should create a new user).
- Then in Swagger click "Authorize" button and paste token in format:
    
        bearer [your token]

## Api Endpoints
- ### Auth
  - **POST /api/auth/login**: Authenticates a user and returns a JWT token.
    - **Input**: `username`, `password`
    - **Output**: `JWT token`, `username`
    - **Access**: For all users

  - **POST /api/auth/register**: Registers a new user with default USER role.
    - **Input**: `firstName`, `lastName`, `username`, `email`, `password`
    - **Output**: -
    - **Access**: For all users

- ### User
  - **GET /api/users**: Retrieves information about all users.
    - **Input**: -
    - **Output**: `List of users`
    - **Access**: Only for administrators

  - **GET /api/user/{username}**: Retrieves information about a specific user.
    - **Input**: -
    - **Output**: `Requested user's data`
    - **Access**: Only for the requested user and administrators

  - **POST /api/user/{username}/update**: Updates the information of the current user.
    - **Input**: New `firstName`, `lastName`, `username`
    - **Output**: -
    - **Access**: Only for the target user and administrators

  - **POST /api/user/{username}/changePassword**: Changes the password of the current user.
    - **Input**: New `password`, old `password`
    - **Output**: -
    - **Access**: Only for the target user and administrators

  - **POST /api/user/{username}/addToRole**: Adds the user to a specified role.
    - **Input**: `Role name`
    - **Output**: -
    - **Access**: Only for administrators

  - **POST /api/user/{username}/removeFromRole**: Removes the user from a specified role.
    - **Input**: `Role name`
    - **Output**: -
    - **Access**: Only for administrators
