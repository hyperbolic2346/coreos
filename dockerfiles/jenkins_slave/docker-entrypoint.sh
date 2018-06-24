#!/bin/bash
set -e

case "$1" in
  image | test | deploy)
    cd /home/jenkins && exec wrapdocker sudo -EHsu jenkins ci-${1}.sh;
esac

exec wrapdocker "$@"
