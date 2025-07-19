# Tainted Grail: The Fall of Avalon Mods

## Development environment setup

### Configuring the Game Directory

The build system copies mod DLLs to the directory set by the `GameDir` property in `Directory.Build.props` (default:
`C:\Program Files (x86)\Steam\steamapps\common\Tainted Grail FoA\`).
It will also deploy the mod DLLs to the `GameDir\BepInEx\plugins\` automatically when you build a mod.

To change it:

1. Open `Global.props` in the repo root.
1. Edit the path: (note the trailing backslash)
   ```xml
   <GameDir>D:\Alternative\Game\Path\</GameDir>
   ```

You can now run `dotnet build` (or build with your favorite IDE) which should automatically copy the game DLLs to the
`lib\` directory under the solution.

See [ModPrebuild.csproj](ModPrebuild/ModPrebuild.csproj) for details on how this works.

### Package a mod

To package a mod for release, run the following script (using git bash):

Build output is saved to `artifacts/{Mod Folder Name}.zip`

```shell
./package.sh {Mod Folder Name}
```

## Update mod version

The mods in this project use [Semantic Versioning](https://semver.org/).

To increase version number of a mod, run the following script (using git bash):

```shell
./bump-version.sh {Mod Folder Name} major # breaking change
./bump-version.sh {Mod Folder Name} minor # new feature
./bump-version.sh {Mod Folder Name} patch # bug fix

# Then follow instructions to commit the version upgrade
```

## Generating a new mod project

```shell
# Install the template
dotnet new install ./templates/mod --force

# Generate the new mod dir
cd NewModFolder
dotnet new tgfoamod --modAuthor "YourName"

# Add the new mod project to the .NET solution
dotnet sln ../FallOfAvalonMods.sln add .
```
