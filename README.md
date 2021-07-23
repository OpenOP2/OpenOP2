An Outpost 2 mod for the [OpenRA](https://github.com/OpenRA/OpenRA) strategy game engine.

You can get the latest release from the [Releases page](https://github.com/OpenOP2/OpenOP2/releases).

You must own [Outpost 2 on GOG](https://www.gog.com/game/outpost_2_divided_destiny) and have it installed to run this mod.

This mod is a work-in-progress. Many features are missing or incomplete at this stage.

Join the [OpenOP2 Discord channel](https://discord.gg/XdsJWKJwmw)

Thanks to 
David Wilson (drogoganor)
Matthias Mailänder (Mailaender)
Zhall
Zon
KOYK (ELoyros)
and the OpenRA developer team.

### Running from a local cloned repository

The key scripts in this SDK are:

| Windows               | Linux / macOS            | Purpose
| --------------------- | ------------------------ | ------------- |
| make.cmd              | Makefile                 | Compiles your project and fetches dependencies (including the OpenRA engine).
| launch-game.cmd       | launch-game.sh           | Launches your project from the SDK directory.
| launch-server.cmd     | launch-server.sh         | Launches a dedicated server for your project from the SDK directory.
| utility.cmd           | utility.sh         | Launches the OpenRA Utility for your project.
| &lt;not available&gt; | packaging/package-all.sh | Generates release installers for your project.

To launch your project from the development environment you must first compile the project by running `make.cmd` (Windows), or opening a terminal in the SDK directory and running `make` (Linux / macOS).  You can then run `launch-game.cmd` (Windows) or `launch-game.sh` (Linux / macOS) to run your game.
