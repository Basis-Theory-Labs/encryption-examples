#!/bin/bash
set -eo pipefail

# verify the software

current_directory="$PWD"

cd $(dirname $0)

time {
    ./dependencycheck.sh
    ./importcert.sh
    ./build.sh
}

cd "$current_directory"
