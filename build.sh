#!/usr/bin/env bash

mono=
fsiargs=

if [[ "$OS" != "Windows_NT" ]]; then
  # Use mono.
  export MONO_MANAGED_WATCHER=false

  mono=mono
  fsiargs=-d:MONO
fi

[[ -f ".paket/paket.exe" ]] || $mono .paket/paket.bootstrapper.exe || exit $?
$mono .paket/paket.exe restore || exit $?
$mono packages/FAKE/tools/FAKE.exe "$@" --fsiargs $fsiargs build.fsx
