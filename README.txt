This program was built using C# with .NET Core 3.1 and a plugin called Newtonsoft.Json, which is a json conversion package for .NET.  I downloaded this package from the NuGet Package Manager.

Newtonsoft.Json is used to convert the request payload string into a Point object.

You can run the program by clicking on the shortcut to KonicaServer.exe in the root of the KonicaServer folder directory.  Alternatively, you can open the solution (KonicaServer.sln) in Visual Studio and run it in debug or release mode.

I elected to run the server with the HTTP API Protocol on http://localhost:8080.  The init.js code of the client must include the following code:
const app = Elm.Main.embed(node, {
    api: 'Http',
    hostname: 'http://localhost:8080',
});

The code is well commented, and you can see my code comments for an explanation of my implementation.  Open KonicaServer.sln in Visual Studio and start in Program.cs to follow the implementation logic.