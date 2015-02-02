# Addon container to map ports via upnp. 
#
# Expects a port as an environment variable. It will curl the port/host and setup upnp if it fails.
#
FROM ubuntu:14.04
MAINTAINER Mike Wilson <knobby@burntsheep.com>

# update
RUN apt-get update && apt-get install -y miniupnpc software-properties-common

ADD files/boot.sh /usr/local/bin/boot.sh
RUN chmod +x /usr/local/bin/boot.sh

ADD files/poll.sh /usr/local/bin/poll.sh
RUN chmod +x /usr/local/bin/poll.sh

CMD ["/usr/local/bin/boot.sh"]

