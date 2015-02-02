#!/bin/sh
die () {
    echo >&2 "$@"
    exit 1
}

[ "$#" -eq 1 ] || die "1 argument required(docker container name), $# provided"

PID=$(docker inspect --format {{.State.Pid}} $1)
nsenter --target $PID --mount --uts --ipc --net --pid
