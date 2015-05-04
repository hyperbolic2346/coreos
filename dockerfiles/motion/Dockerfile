# Motion
#
FROM ubuntu:14.04
MAINTAINER Mike Wilson <knobby@burntsheep.com>

# update
RUN apt-get update && apt-get install -y motion python2.7 php5-cli php5-gearman software-properties-common

# replace motion with our compiled version
RUN rm /usr/bin/motion
ADD files/motion /usr/bin/motion

ADD https://github.com/kelseyhightower/confd/releases/download/v0.6.3/confd-0.6.3-linux-amd64 /usr/local/bin/confd
RUN chmod +x /usr/local/bin/confd

ADD files/confd-watch /usr/local/bin/confd-watch
RUN chmod +x /usr/local/bin/confd-watch

#ADD files/motion.toml /etc/confd/conf.d/motion.toml
#ADD files/motion.tmpl /etc/confd/conf.d/motion.tmpl

ADD files/motion.default.conf /etc/default/motion
RUN echo America/New_York > /etc/timezone && dpkg-reconfigure --frontend noninteractive tzdata

CMD ["/usr/local/bin/confd-watch"]

