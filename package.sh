#!/bin/bash
set -e

SCRIPT_DIR=$(dirname "$(readlink -f "$0")")

modDir="$1"
if [[ -z "$modDir" ]]; then
    echo "Usage: $0 <modDir>"
    exit 1
fi

dotnet restore "$modDir"
dotnet build -c Release "$modDir"

if [[ ! -d "$modDir" ]]; then
    echo "Directory $modDir does not exist"
    exit 1
fi

if [[ ! -f "$modDir/bin/Release/net6.0/$modDir.dll" ]]; then
    echo "Build output does not exist"
    exit 1
fi

mkdir -p "$SCRIPT_DIR/artifacts"

# Find the last released tag for the mod (tags are $modDir-$version, but modDir lowercased)
lower_moddir=$(echo "$modDir" | tr '[:upper:]' '[:lower:]')

# Get all tags for this mod, sorted by version descending
mod_tags=($(git tag --list "${lower_moddir}-*" --sort=-v:refname))

# Check if HEAD is at a release tag for this mod
current_tag=$(git tag --points-at HEAD | grep "^${lower_moddir}-" | head -n 1)
if [[ -z "$current_tag" ]]; then
    echo "Error: HEAD is not a release tag for $modDir."
    echo "       Please run the following command, and follow the instructions:"
    echo "         ./bump-version.sh $modDir (major|minor|patch)"
    exit 1
fi

echo "Packaging at release tag: $current_tag"

if [[ ${#mod_tags[@]} -lt 2 ]]; then
    prev_tag=""
else
    prev_tag="${mod_tags[1]}"
fi

echo "Previous release tag for $modDir: $prev_tag"

changelog_path="$SCRIPT_DIR/artifacts/${modDir}_CHANGELOG.txt"

if [[ -z "$prev_tag" ]]; then
    echo "No previous tag found for $modDir. Creating initial changelog."
    echo "Initial version" > "$changelog_path"
else
    git log --pretty=format:"%h %s (%an, %ad)" --date=format:'%Y-%m-%d %H:%M' $prev_tag..$current_tag -- "$modDir" > "$changelog_path"
fi


echo
echo "========="
echo "Changelog for $modDir:"
echo "========="
cat "$changelog_path"
echo
echo

rm -f "$SCRIPT_DIR/artifacts/${modDir}.zip"
7za a -tzip "$SCRIPT_DIR/artifacts/${modDir}.zip" "$SCRIPT_DIR/$modDir/bin/Release/net6.0/${modDir}.dll" "$changelog_path" | grep -i "archive"
