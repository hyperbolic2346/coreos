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

OPENHAB_CONF_DIR="/etc/openhab"
OPENHAB_DIR="/opt/openhab"
OPENHAB_WORKSPACE_DIR="/var/lib/openhab/workspace"
OPENHAB_LOG_DIR="/var/log/openhab"
JAVA="/opt/jdk7/bin/java"

LAUNCHER=`ls ${OPENHAB_DIR}/server/plugins/org.eclipse.equinox.launcher_*.jar`

# Exit if the package is not installed
if [ ! -r "$LAUNCHER" ]; then
	echo "launcher jar is missing"
	exit 5
fi

JAVA_ARGS_DEFAULT="-Dosgi.clean=true \
 -Declipse.ignoreApp=true \
 -Dosgi.noShutdown=true \
 -Djetty.port=8080 \
 -Dopenhab.logdir="${OPENHAB_LOG_DIR}" \
 -Dlogback.configurationFile="${OPENHAB_CONF_DIR}/configurations/logback.xml" \
 -Dopenhab.configfile="${OPENHAB_CONF_DIR}/configurations/openhab.cfg" \
 -Dopenhab.configdir="${OPENHAB_CONF_DIR}/configurations" \
 -Djetty.home="${OPENHAB_DIR}" \
 -Djetty.port.ssl=8443 \
 -Djetty.config="${OPENHAB_CONF_DIR}/jetty" \
 -Djetty.rundir="${OPENHAB_DIR}" \
 -Dfelix.fileinstall.dir="${OPENHAB_DIR}/addons" \
 -Djava.library.path="${OPENHAB_DIR}/lib" \
 -Djava.security.auth.login.config="${OPENHAB_CONF_DIR}/login.conf" \
 -Dorg.quartz.properties="${OPENHAB_CONF_DIR}/quartz.properties" \
 -Dequinox.ds.block_timeout=240000 \
 -Dequinox.scr.waitTimeOnBlock=60000 \
 -Dfelix.fileinstall.active.level=4 \
 -Djava.awt.headless=true \
 -jar ${LAUNCHER} \
 -data ${OPENHAB_WORKSPACE_DIR} \
 -console"

$JAVA $JAVA_ARGS_DEFAULT
