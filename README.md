# youtube-websocket
Sample project of how to use websockets in ASP.NET Core inspired from youtube video series https://youtu.be/2QS55uqcRrg

# Content
Inside the repositoy exists two folders, 'Youtube.Websocket' and 'Youtube.Websocket.ConsoleApp'.
The first one contains the ASP.NET Core project that receive and send messages using WebSockets.
The second one contains a console application for testing the messaging system.

# How to run
If you decided to execute the ASP.NET Core project from visual studio, it will listen to request on port 57835. But, if you decided to execute this project from the Powershell console using the command 'dotnet run', then the service will listen on port 50000 by default.

To execute the console application you have to consider the option used above to execute the web service. The go to line 28 and comment or uncomment the right option in the Program.cs file.

That's it!