#!/bin/bash
set -e

current_directory="$PWD"

cd $(dirname $0)/../src/01-no-encryption

dotnet restore

dotnet build NoEncryption.sln --no-restore -c Release

result=$?

cd $(dirname $0)/../02-basic-encryption

dotnet restore

dotnet build BasicEncryption.sln --no-restore -c Release

result=$?

cd $(dirname $0)/../03-shared-encryption

dotnet restore

dotnet build SharedEncryption.sln --no-restore -c Release

result=$?

cd $(dirname $0)/../04-key-rotation

dotnet restore

dotnet build KeyRotation.sln --no-restore -c Release

result=$?

cd $(dirname $0)/../05-openkms-encryption

dotnet restore

dotnet build OpenKmsEncryption.sln --no-restore -c Release

result=$?

cd "$current_directory"

exit $result
