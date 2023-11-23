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
  - **Login()**. Issues a Bearer token to valid users. This token can be used to authorize in this AuthService.
  - **ExternalLogin()**. Works only for authorized users. Issues a Bearer token for authorization in other applications that use this AuthService as authentication server.
 
- ### User
  - **GetCurrentUserInfo()**. Works only for authorized users. Returns information about the current user.
  - **GetAllUsersInfo()**. Works only for ADMINs. Returns information about all registered users.
  - **Register()**. Authorization is not required, works for everyone. Registers a new user with defaul USER role.
  - **UpdateCurrentUserInfo()**. Works only for authorized users. Updates current user information.
  - **ChangeCurrentUserPassword()**. Works only for authorized users. Changes current user information.
  - **AddAdminRoleToUser()**. Works only for ADMINs. Makes given user an Admin.
