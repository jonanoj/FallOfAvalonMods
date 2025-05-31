# Tainted Grail: The Fall of Avalon Mods

## Development environment setup

### Configuring the Game Directory

The build system copies mod DLLs to the directory set by the `GameDir` property in `Global.props` (default:
`C:\Program Files (x86)\Steam\steamapps\common\Tainted Grail FoA\`).
It will also deploy the mod DLLs to the `GameDir\BepInEx\plugins\` automatially when you build a mod.

To change it:

1. Open `Global.props` in the repo root.
1. Edit the path: (note the trailing backslash)
   ```xml
   <GameDir>D:\Alternative\Game\Path\</GameDir>
   ```

### Acquiring the game DLLs

1. Install [BepInEx](https://github.com/BepInEx/BepInEx) v6.0.0 Unity IL2CPP version, either
    1. The official [Bleeding Edge build](https://builds.bepinex.dev/projects/bepinex_be) as of writing this guide,
       version #735 was used. (BepInEx 6.0.0-pre.2 doesn't support the game)
    1. The [Nexus Mods package](https://www.nexusmods.com/taintedgrailthefallofavalon/mods/16)
1. Run the game at least once, and wait until the game DLLs are fully dumped to `GameDir\BepInEx\interop\`

You can now run `dotnet build` which should automatically copy the game DLLs to the `lib\` directory under the solution.

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
