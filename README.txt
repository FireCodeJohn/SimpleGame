FULL DOCUMENTATION:

The full Documentation is in technical-assessment/documentation.pdf and is VERY long

HOW TO RUN THE GAME ON WINDOWS:   

1) Download the code as a zip and unzip it.
2) Start the server by running SimpleGame-main\Server\bin\Release\netcoreapp3.1\publish\KonicaServer.exe
3) Start the client by opening SimpleGame-main\technical-assessment\client\index.html
4) Play the game (2 player) by drawing lines according the the rules described in the documentation file.  Last one to draw a line loses.

WHAT I DID:
Everything inside the Server folder was made by me using Visual Studio.  The simple server connects to the client and saves a game state, applying all the rules and logic of the game.  This was a good project for me, it really allowed me to show my logical skillset to solve problems in software development with C# and .NET.  This took me 10-20 hours, over the course of 2 weekends (its hard to remember exactly how long it took me, as this was made over a year ago).

WHAT I DID NOT DO:
Everything inside the technical-assessment folder was given to me as a prompt.  The simple client was not created by me.  The client is a simple grid drawing tool that does not implement any game rules and does not have any knowledge of the game state.  See the full documentation for more details.

MY ORIGINAL COMMENTS:
This program was built using C# with .NET Core 3.1 and a plugin called Newtonsoft.Json, which is a json conversion package for .NET.  I downloaded this package from the NuGet Package Manager.

Newtonsoft.Json is used to convert the request payload string into a Point object.

You can run the program by clicking on the shortcut to KonicaServer.exe in the root of the KonicaServer folder directory.  Alternatively, you can open the solution (KonicaServer.sln) in Visual Studio and run it in debug or release mode.

I elected to run the server with the HTTP API Protocol on http://localhost:8080.  The init.js code of the client must include the following code:
const app = Elm.Main.embed(node, {
    api: 'Http',
    hostname: 'http://localhost:8080',
});

The code is well commented, and you can see my code comments for an explanation of my implementation.  Open KonicaServer.sln in Visual Studio and start in Program.cs to follow the implementation logic.
