# Tainted Grail: The Fall of Avalon Mods

## Development environment setup
### Acquiring the game DLLs
1. Install [BepInEx](https://github.com/BepInEx/BepInEx) v6.0.0 Unity IL2CPP version (as of writing this guide, stable builds don't support the game yet, I've used [Bleeding Edge build #735](https://builds.bepinex.dev/projects/bepinex_be))
1. Run the game at least once
1. Create a `lib/` folder under the root dir of the project
1. Copy `TG.Main.dll` to `lib/` (and additional DLLs if needed) from the path `GameDirectory/BepInEx/interop/TG.Main.dll`

### Configuring the Game Directory

The build system copies mod DLLs to the directory set by the `GameDir` property in `Directory.Build.props` (default: `C:\Program Files (x86)\Steam\steamapps\common\Tainted Grail FoA`).

**To change it:**
1. Open `Directory.Build.props` in the repo root.
1. Edit the path:
   ```xml
   <GameDir>D:\Alternative\Game\Path</GameDir>
   ```

### Build a mod
```shell
./build.sh {Mod Folder Name}

# Build output is saved to artifacts/{Mod Folder Name}.zip
```

## Update mod version
```shell
./bump-version.sh {Mod Folder Name} major # breaking change
./bump-version.sh {Mod Folder Name} minor # new feature
./bump-version.sh {Mod Folder Name} patch # patch

# Then follow instructions to commit the version upgrade
```

## Generating a new mod
```shell
# Install the template
dotnet new install ./templates/mod --force

# Generate the new mod dir
cd NewModFolder
dotnet new tgfoamod --modAuthor "YourName"

# Add the new mod project to the .NET solution
dotnet sln ../FallOfAvalonMods.sln add .
```
