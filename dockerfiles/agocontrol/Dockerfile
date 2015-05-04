# Agocontrol
# * configuration is injected
#
FROM debian
MAINTAINER Mike Wilson <knobby@burntsheep.com>

RUN apt-get -y install wget

RUN wget -O - http://mirror.at.agocontrol.com/debian/conf/agocontrol.gpg.key | apt-key add -
RUN echo "deb http://mirror.at.agocontrol.com/debian testing main" > /etc/apt/sources.list.d/ago.list
RUN apt-get update && apt-get install agocontrol

ADD files/boot.sh /usr/local/bin/boot.sh
RUN chmod +x /usr/local/bin/boot.sh

# Clean up
RUN rm -rf /tmp/*

EXPOSE 8080 8443 5555 9001

CMD ["/usr/local/bin/boot.sh"]

