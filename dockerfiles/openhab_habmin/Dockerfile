# Openhab 1.5.1
# * configuration is injected
#
FROM tdeckers/openhab
MAINTAINER Mike Wilson <knobby@burntsheep.com>

# update
RUN apt-get update && apt-get install -y git

RUN mkdir -p /opt/openhab/webapps/habmin
RUN git clone https://github.com/cdjackson/HABmin.git /opt/openhab/webapps/habmin

#ADD https://github.com/cdjackson/HABmin/releases/download/0.1.3-snapshot/habmin.zip /tmp/habmin.zip
#RUN mkdir -p /opt/openhab/webapps/habmin
#RUN unzip -d /opt/openhab/webapps/habmin /tmp/habmin.zip

RUN mv /opt/openhab/webapps/habmin/addons/* /opt/openhab/addons-avail

# remove pipework
RUN rm /usr/local/bin/pipework
RUN rm /usr/local/bin/boot.sh

ADD files/boot.sh /usr/local/bin/boot.sh
RUN chmod +x /usr/local/bin/boot.sh

RUN echo America/New_York > /etc/timezone && dpkg-reconfigure --frontend noninteractive tzdata

# Clean up
RUN rm -rf /tmp/*

EXPOSE 8080 8443 5555 9001

CMD ["/usr/local/bin/boot.sh"]

