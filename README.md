# Tainted Grail: The Fall of Avalon Mods

## Build a mod
```shell
./build.sh {Mod Folder Name}
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