#!/bin/bash

dotnet tool install --global AppSettingsDocGenerator
export PATH="$PATH:/root/.dotnet/tools"

cd /source

appsettings_generator "$@"