#!/bin/bash

echo "###########################################################################"
echo "# Hurtworld Server - " `date`
echo "###########################################################################"
[ -p /tmp/FIFO ] && rm /tmp/FIFO
mkfifo /tmp/FIFO

export TERM=linux

if [ ! -w /hurtworld ]; then
   echo "[Error] Unable to access hurtworld directory. Check permissions on your mapped directory to /hurtworld"
   exit 1
fi

cd /hurtworld

# make sure we have these symlinks(could be a new volume)
[ ! -f hwserver ] && [ ! -L hwserver ] && ln -s /linuxgsm/hwserver hwserver
[ ! -f lgsm-default.cfg ] && [ ! -L lgsm-default.cfg ] && ln -s /linuxgsm/Hurtworld/cfg/lgsm-default.cfg lgsm-default.cfg
[ ! -f crontab ] && cp /defaults/crontab crontab

if [ ! -d "serverfiles" ] || [ ! -f "serverfiles/Hurtworld.x86_64" ]; then
  echo "Installing..."
  hwserver auto-install
else
  if [ ${UPDATEONSTART} -eq 1 ]; then
    echo "Updating..."
    hwserver update
  fi
fi

[ ! -f log ] && [ ! -L log ] && ln -s /linuxgsm/Hurtworld/log log

# If there is uncommented line in the file
CRONNUMBER=`grep -v "^#" crontab | wc -l`
if [ $CRONNUMBER -gt 0 ]; then
	echo "Loading crontab..."
	# We load the crontab file if it exist.
	crontab /hurtworld/crontab
	# Cron is attached to this process
	sudo cron -f &
else
	echo "No crontab set."
fi

# Start the hurtworld service using the generated config

echo "[hurtworld] starting game server..."
hwserver start

# Stop server in case of signal INT or TERM
echo "Waiting..."
trap 'hwserver stop;' INT
trap 'hwserver stop' TERM

read < /tmp/FIFO &
wait
