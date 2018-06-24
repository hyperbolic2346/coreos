FROM ubuntu:latest
MAINTAINER Mike Wilson <knobby@burntsheep.com>

RUN mkdir -p /srv
ADD files/bf2.tgz /srv/

WORKDIR /srv/bf2

ENV TERM=xterm

CMD ["/srv/bf2/start.sh"]
