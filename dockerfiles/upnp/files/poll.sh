#!/bin/bash

# forward_port(port_num, UDP|TCP, my_ip)
forward_port() {
  if [ "$2" = "UDP" ]; then
    echo "redirecting udp port $1 to $3"
    upnpc -d $1 udp
    upnpc -a $3 $1 $1 udp
  else
    echo "redirecting tcp port $1 to $3"
    upnpc -d $1 tcp
    upnpc -a $3 $1 $1 tcp
  fi
}

# Find our IP
my_ip=$(ip addr | grep 'state UP' -A2 | grep '10.0.1.' | awk '{print $2}' | cut -f1  -d'/')

if [[ "$UDP_FORWARDING_ONLY" = "yes" || "$UDP_FORWARDING_TOO" = "yes" ]]; then
  echo "checking udp port $PORT_TO_MONITOR..."
  if [[ $(upnpc -l|grep libminiupnpc|grep " $PORT_TO_MONITOR"|grep UDP|grep -c "$my_ip") -eq 0 ]]; then
    forward_port $PORT_TO_MONITOR "UDP" $my_ip
  fi
fi

if [ "$UDP_FORWARDING_ONLY" != "yes" ]; then
  echo "checking tcp port $PORT_TO_MONITOR..."
  if [[ $(upnpc -l|grep libminiupnpc|grep " $PORT_TO_MONITOR"|grep TCP|grep -c "$my_ip") -eq 0 ]]; then
    forward_port $PORT_TO_MONITOR "TCP" $my_ip
  fi
fi
