****# .NET 8 AOT Support with Database Access

This sample project uses the Native AOT support found in .NET 8. I've altered the template to use a SQLite database to show that it's possible to do work with AOT.

Support is still very limited, and going down this route likely means you have to give up on some of your favorite packages currently in the .NET ecosystem. You might see some community support, but my guess it will be closer to the .NET 9 or 10 release.

Start up and execution are fast, but the code you'll write will be very verbose and explicit. Trade-offs and all that.

**Note: You will need .NET 8 SDK**

