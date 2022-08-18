#!/bin/bash
set -e

current_directory="$PWD"

cd $(dirname $0)/..

if [ "$(uname)" == "Darwin" ]; then
  if ! security find-certificate -c bank-keyvault-emulator > /dev/null; then
    sudo security add-trusted-cert -d -r trustRoot -k /Library/Keychains/System.keychain src/local-certs/bank-keyvault-emulator.crt
  else
    echo "bank-keyvault-emulator certificate already installed"
  fi
elif [ "$(expr substr $(uname -s) 1 5)" == "Linux" ]; then
  sudo cp src/local-certs/bank-keyvault-emulator.crt /usr/local/share/ca-certificates/bank-keyvault-emulator.crt
  sudo update-ca-certificates
fi

result=$?

cd "$current_directory"

exit $result
