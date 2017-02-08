#!/usr/bin/env bash

mono=

if [[ "$OS" != "Windows_NT" ]]; then
  mono=mono
fi

[[ -f ".paket/paket.exe" ]] || $mono .paket/paket.bootstrapper.exe || exit $?
$mono .paket/paket.exe "$@"
