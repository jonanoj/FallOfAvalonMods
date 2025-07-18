#!/bin/bash
set -e

SCRIPT_DIR=$(dirname "$(readlink -f "$0")")

modDir="$1"
if [[ -z "$modDir" ]]; then
    echo "Usage: $0 <modDir>"
    exit 1
fi

dotnet build -c Release "$modDir"

if [[ ! -d "$modDir" ]]; then
    echo "Directory $modDir does not exist"
    exit 1
fi

MOD_DLL_PATH="$modDir/bin/Release/netstandard2.1/$modDir.dll"
if [[ ! -f "$MOD_DLL_PATH" ]]; then
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
    echo "No previous tag for $modDir, not creating changelog."
else
    git log --pretty=format:"%h %s (%an, %ad)" --date=format:'%Y-%m-%d %H:%M' $prev_tag..$current_tag -- "$modDir" > "$changelog_path"

    echo
    echo "========="
    echo "Changelog for $modDir:"
    echo "========="
    cat "$changelog_path"
    echo
fi

package_zip() {
    local zip_path="$1"
    local variation_dir="$2" # May be empty for no variation

    echo
    echo "Packaging $modDir into $zip_path"

    local tmpdir=$(mktemp -d)
    mkdir -p "$tmpdir/plugins"
    cp "$SCRIPT_DIR/$MOD_DLL_PATH" "$tmpdir/plugins/"
    if [[ -f "$changelog_path" ]]; then
        cp "$changelog_path" "$tmpdir/plugins/"
    fi
    if [[ -n "$variation_dir" ]]; then
        cp -r "$variation_dir/." "$tmpdir/"
    fi

    # Remove all .gitkeep files from the temp dir before zipping
    find "$tmpdir" -type f -name ".gitkeep" -delete

    rm -f $zip_path
    (cd "$tmpdir" && 7za a -tzip "$zip_path" ./* | grep -i "archive")
    rm -rf "$tmpdir"
}

if [[ -d "$modDir/variations" ]]; then
    echo "Found variations for $modDir. Packaging each variation."
    for variation in "$modDir/variations"/*; do
        if [[ -d "$variation" ]]; then
            var_name=$(basename "$variation")
            package_zip "$SCRIPT_DIR/artifacts/${modDir}-${var_name}.zip" "$variation"
        fi
    done
else
    package_zip "$SCRIPT_DIR/artifacts/${modDir}.zip" ""
fi
