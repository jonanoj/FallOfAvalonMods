#!/bin/bash
set -e

modDir="$1"
versionPart="$2"

if [[ -z "$modDir" || -z "$versionPart" ]]; then
    echo "Usage: $0 <modDir> <major|minor|patch>"
    exit 1
fi

constsPath="$modDir/PluginConsts.cs"
if [[ ! -f "$constsPath" ]]; then
    echo "PluginConsts.cs not found in $modDir"
    exit 1
fi

# Extract and bump version in PluginConsts.cs
verLine=$(grep -E 'PLUGIN_VERSION[[:space:]]*=[[:space:]]*"[0-9]+\.[0-9]+\.[0-9]+";' "$constsPath")
if [[ $verLine =~ PLUGIN_VERSION[[:space:]]*=[[:space:]]*\"([0-9]+)\.([0-9]+)\.([0-9]+)\" ]]; then
    major=${BASH_REMATCH[1]}
    minor=${BASH_REMATCH[2]}
    patch=${BASH_REMATCH[3]}
    case "$versionPart" in
    major)
        major=$((major + 1))
        minor=0
        patch=0
        ;;
    minor)
        minor=$((minor + 1))
        patch=0
        ;;
    patch)
        patch=$((patch + 1))
        ;;
    *)
        echo "Invalid version part: $versionPart"
        exit 1
        ;;
    esac
    newVersion="$major.$minor.$patch"
    # Replace the version line (with semicolon)
    sed -i -E "s/(PLUGIN_VERSION[[:space:]]*=[[:space:]]*)\"[0-9]+\.[0-9]+\.[0-9]+\";/\1\"$newVersion\";/" "$constsPath"
    echo "Version bumped to $newVersion in PluginConsts.cs"
else
    echo "PLUGIN_VERSION not found in $constsPath"
    exit 1
fi

# Update csproj version
csprojPath="$modDir/$modDir.csproj"
if [[ -f "$csprojPath" ]]; then
    sed -i -E "s|<Version>.*</Version>|<Version>$newVersion</Version>|" "$csprojPath"
    echo "Updated csproj version to $newVersion"
else
    echo "$modDir.csproj not found in $modDir"
fi

newVersionTag=$(echo "$modDir" | tr '[:upper:]' '[:lower:]')-$newVersion

cat <<EOF

###########
# Please run the following commands to commit and tag the changes:
###########
cd "$modDir"
git add PluginConsts.cs $modDir.csproj
git commit -m 'Bump version to $newVersion'
git push origin main
git tag $newVersionTag
git push origin $newVersionTag
EOF
