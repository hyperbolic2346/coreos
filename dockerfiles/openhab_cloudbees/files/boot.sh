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
cd /opt/openhab
/opt/jdk7/bin/java -Dosgi.clean=true -Declipse.ignoreApp=true -Dosgi.noShutdown=true -Djetty.port=8080 -Djetty.port.ssl=8443 -Djetty.home=. -Dlogback.configurationFile=configurations/logback.xml -Dfelix.fileinstall.dir=addons -Djava.library.path=lib -Djava.security.auth.login.config=./etc/login.conf -Dorg.quartz.properties=./etc/quartz.properties -Dequinox.ds.block_timeout=240000 -Dequinox.scr.waitTimeOnBlock=60000 -Dfelix.fileinstall.active.level=4 -Djava.awt.headless=true -jar ./server/plugins/org.eclipse.equinox.launcher_1.3.0.v20120522-1813.jar -console
