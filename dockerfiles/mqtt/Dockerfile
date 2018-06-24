# Mqtt server - mosquitto
#
# Expects that you map the config file into /etc/mosquitto/mosquitto.conf
#
FROM ubuntu:14.04
MAINTAINER Mike Wilson <knobby@burntsheep.com>

# update
RUN apt-get update && apt-get install -y software-properties-common

RUN apt-add-repository ppa:mosquitto-dev/mosquitto-ppa

RUN apt-get update && apt-get install -y mosquitto

#ADD files/boot.sh /usr/local/bin/boot.sh
#RUN chmod +x /usr/local/bin/boot.sh

EXPOSE 8883

#CMD ["/usr/local/bin/boot.sh"]
CMD ["/usr/sbin/mosquitto", "-c", "/etc/mosquitto/mosquitto.conf"]
