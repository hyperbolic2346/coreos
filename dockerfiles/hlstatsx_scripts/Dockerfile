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

ADD files/cronfile /etc/cron.d/hlstatsx
ADD files/hlstats.conf /usr/src/hlstatsx/hlstats.conf

RUN echo America/New_York > /etc/timezone && dpkg-reconfigure --frontend noninteractive tzdata

EXPOSE 27500/udp

WORKDIR /usr/src/hlstatsx

CMD ["./hlstats.pl", "--configfile=hlstats.conf", "--port=27500"]
