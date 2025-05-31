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

# Only package the .dll file using 7za, with no nested dirs in the zip
rm -f "$SCRIPT_DIR/artifacts/${modDir}.zip"
7za a -tzip "$SCRIPT_DIR/artifacts/${modDir}.zip" "$SCRIPT_DIR/$modDir/bin/Release/net6.0/${modDir}.dll"
