# HLStatx web page
# 
# Created from https://github.com/jaredballou/insurgency-hlstatsx
#
FROM perl:5.20
MAINTAINER Mike Wilson <knobby@burntsheep.com>

RUN apt-get update && apt-get install -yq git cron libdbd-mysql-perl libdbi-perl && rm -rf /var/lib/apt/lists/*

RUN mkdir /tmp/hlstatsx && git clone https://github.com/jaredballou/insurgency-hlstatsx.git /tmp/hlstatsx
RUN mv /tmp/hlstatsx/scripts /usr/src/hlstatsx
RUN rm -rf /tmp/hlstatsx

ADD https://github.com/kelseyhightower/confd/releases/download/v0.6.3/confd-0.6.3-linux-amd64 /usr/local/bin/confd
RUN chmod +x /usr/local/bin/confd

ADD files/cronfile /etc/con.d/hlstatsx

ADD files/confd-watch /usr/local/bin/confd-watch
RUN chmod +x /usr/local/bin/confd-watch

RUN echo America/New_York > /etc/timezone && dpkg-reconfigure --frontend noninteractive tzdata

CMD ["/usr/local/bin/confd-watch"]
