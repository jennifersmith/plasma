#!/bin/bash
$WINDIR/Microsoft.Net/Framework/v4.0.30319/MSBuild.exe Plasma.proj /verbosity:n  $@
if [ "$?" -ne 0 ]; then
    echo $'\E[30;41m'
    cat tools/buildflags/failed.txt
    echo $'\E[0m'

    exit 1
else
    echo $'\E[30;42m'
    cat tools/buildflags/passed.txt
    echo $'\E[0m'
    exit 0
fi