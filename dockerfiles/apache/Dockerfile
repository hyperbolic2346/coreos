# Gearman
#
FROM ubuntu:14.04
MAINTAINER Mike Wilson <knobby@burntsheep.com>

# update
RUN apt-get update && apt-get install -y git apache2

RUN mkdir -p /var/www/gate
RUN git clone https://github.com/hyperbolic2346/gate.git /var/www/gate

ADD files/config.inc /var/www/gate/docs/config.inc

EXPOSE 8080 

CMD ["/usr/sbin/apache2ctl -D FOREGROUND"]

