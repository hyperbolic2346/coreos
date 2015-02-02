# Gearman
#
FROM ubuntu:14.04
MAINTAINER Mike Wilson <knobby@burntsheep.com>

# update
RUN apt-get update && apt-get install -y supervisor php5-cli php5-gearman libav-tools imagemagick

ADD files/create_videos.php /usr/local/bin/create_videos.php
RUN chmod +x /usr/local/bin/create_videos.php

ADD https://github.com/kelseyhightower/confd/releases/download/v0.6.3/confd-0.6.3-linux-amd64 /usr/local/bin/confd
RUN chmod +x /usr/local/bin/confd

ADD files/supervisord.conf /etc/supervisor/supervisord.conf
ADD files/gearman-client.conf /etc/supervisor/conf.d/gearman-client.conf

RUN mkdir -p /etc/confd/conf.d /etc/confd/templates

ADD files/confd-watch /usr/local/bin/confd-watch
RUN chmod +x /usr/local/bin/confd-watch

CMD ["/usr/local/bin/confd-watch"]
