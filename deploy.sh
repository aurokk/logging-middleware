#!/bin/bash

# Get script directory
DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )";

# Clear
rm -rf $DIR/.output

# Pack
dotnet pack --version-suffix "" --output $DIR/.output $DIR/src/LoggingMiddleware/LoggingMiddleware.csproj

# Deploy
dotnet nuget push "$DIR/.output/*.nupkg" --api-key $NUGET_API_KEY --source https://api.nuget.org/v3/index.json