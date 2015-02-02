# Gearman
#
FROM ubuntu:14.04
MAINTAINER Mike Wilson <knobby@burntsheep.com>

# update
RUN apt-get update && apt-get install -y nginx-extras miniupnpc software-properties-common

ADD https://github.com/kelseyhightower/confd/releases/download/v0.6.3/confd-0.6.3-linux-amd64 /usr/local/bin/confd
RUN chmod +x /usr/local/bin/confd

RUN mkdir -p /etc/confd/conf.d /etc/confd/templates

#ADD files/nginx.toml /etc/confd/conf.d/nginx.toml
#ADD files/nginx.tmpl /etc/confd/templates/nginx.tmpl

ADD files/confd-watch /usr/local/bin/confd-watch
RUN chmod +x /usr/local/bin/confd-watch

RUN rm /etc/nginx/sites-enabled/default

CMD ["/usr/local/bin/confd-watch"]

