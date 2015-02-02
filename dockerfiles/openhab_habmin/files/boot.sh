#!/bin/bash

####################
# Configure timezone

TIMEZONEFILE=/opt/openhab/configurations/timezone

if [ -f "$TIMEZONEFILE" ]
then
  cp $TIMEZONEFILE /etc/timezone
  dpkg-reconfigure -f noninteractive tzdata
fi

###########################
# Configure Addon libraries

SOURCE=/opt/openhab/addons-avail
DEST=/opt/openhab/addons
ADDONFILE=/opt/openhab/configurations/addons.cfg

function addons {
  # Remove all links first
  rm $DEST/*

  # create new links based on input file
  while read STRING
  do
    echo Processing $STRING...
    if [ -f "$SOURCE/$STRING" ]
    then
      ln -s $SOURCE/$STRING $DEST/$STRING
      echo link created.
    else
      echo not found.
    fi
  done < "$ADDONFILE"
}

if [ -f "$ADDONFILE" ]
then
  addons
else
  echo addons.cfg not found.
fi

######################
# Launch

# We're in a container with regular eth0 (default)
exec /usr/bin/supervisord -c /etc/supervisor/supervisord.conf
