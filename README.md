# SignalRWithVuejsExample
The SignalRWithVuejsExample project aims on providing a simple example of a web application using websockets.

The front-end application is developed using Vue.js while the back-end application (API) is developed using .Net Core (C#).

The websockets are provided by the SignalR library.

There is no database for this application. It means the data is lost when the back-end application is restarted.

# Steps to run
## Client with Vue.js

1. Open the client folder
2. Run the command `npm install` to restore the node modules (dependencies)
3. Run the command `npm run serve` to start serving the front-end application

## API with .Net Core

### Running with Visual Studio Code
1. Open the signalrexample.api folder
2. Run the command `dotnet run` to start serving the API

### Running with Vsual Studio
1. Open the SignalRExample.API.sln solution
2. (Re)Build with Ctrl+Shift+B. This will also restore the dependencies (packages)
2. Run the SignalRExample.API project