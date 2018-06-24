#!/bin/bash

#Search for config files, if they don't exist, copy the default ones
#if [ ! -f /config/apache.conf ]; then
#  echo "copying apache.conf"
#  cp /defaults/apache.conf /config/apache.conf
#else
#  echo "apache.conf already exists"
#fi
  
if [ ! -f /config/php/php_cli.ini ]; then
  echo "copying php cli ini"
  cp /defaults/php_cli.ini /config/php/php_cli.ini
else
  echo "php cli ini already exists"
fi

if [ ! -f /config/php/php_apache2.ini ]; then
  echo "copying php apache ini"
  cp /defaults/php_apache2.ini /config/php/php_apache2.ini
else
  echo "php apache ini already exists"
fi
 
if [ ! -f /config/zm.conf ]; then
  echo "copying zm.conf"
  cp /defaults/zm.conf /config/zm.conf
else
  echo "zm.conf already exists"
fi

source /config/zm.conf

result=$(mysql -h $ZM_DB_HOST -u $ZM_DB_USER -p $ZM_DB_PASS -s -N -e "SELECT SCHEMA_NAME FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME='$ZB_DB_NAME'")
if [ -z "$result" ]; then
  echo "db does not exist, creating..."
  mysql -h $ZM_DB_HOST -u $ZM_DB_USER -p $ZM_DB_PASS < /usr/share/zoneminder/db/zm_create.sql
fi

# Create data folder if it doesn't exist
if [ ! -d /config/data ]; then
  echo "creating data folder in config folder"
  mkdir -p /config/data
  cp -r /defaults/data/* /config/data/
else
  echo "using existing data directory"
fi
  
if [ ! -d /config/perl5 ]; then
  echo "moving perl data folder to config folder"
  mkdir /config/perl5
  cp -R -p /defaults/ZoneMinder /config/perl5/
else
  echo "using existing perl data directory"
fi

  
echo "creating symbolink links"
ln -s /config/perl5/ZoneMinder /usr/share/perl5/ZoneMinder
chmod -R go+rw /config
  
#Get docker env timezone and set system timezone
#echo "setting the correct local time"
#echo $TZ > /etc/timezone
#export DEBCONF_NONINTERACTIVE_SEEN=true DEBIAN_FRONTEND=noninteractive
#dpkg-reconfigure tzdata
  
#fix memory issue
#echo "increasing shared memory"
#umount /dev/shm
#mount -t tmpfs -o rw,nosuid,nodev,noexec,relatime,size=${MEM:-4096M} tmpfs /dev/shm
  
/usr/bin/zmpkg.pl start
