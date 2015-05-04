# HLStatx web page
# 
# Created from https://github.com/jaredballou/insurgency-hlstatsx
#
FROM tutum/apache-php
MAINTAINER Mike Wilson <knobby@burntsheep.com>

RUN apt-get update && apt-get install -yq git && rm -rf /var/lib/apt/lists/*
RUN rm -fr /app /var/www/html

RUN mkdir /tmp/hlstatsx && git clone https://github.com/jaredballou/insurgency-hlstatsx.git /tmp/hlstatsx
RUN mv /tmp/hlstatsx/web /var/www/html
RUN rm -rf /tmp/hlstatsx

ADD https://github.com/kelseyhightower/confd/releases/download/v0.6.3/confd-0.6.3-linux-amd64 /usr/local/bin/confd
RUN chmod +x /usr/local/bin/confd

RUN mkdir -p /etc/confd/conf.d /etc/confd/templates

ADD files/confd-watch /usr/local/bin/confd-watch
RUN chmod +x /usr/local/bin/confd-watch

RUN echo America/New_York > /etc/timezone && dpkg-reconfigure --frontend noninteractive tzdata

CMD ["/usr/local/bin/confd-watch"]
