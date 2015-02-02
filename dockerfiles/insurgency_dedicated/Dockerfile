FROM ubuntu:latest
MAINTAINER Mike Wilson <knobby@burntsheep.com>

RUN apt-get update && apt-get install -yq gdb mailutils postfix lib32gcc1 cron tmux python ca-certificates net-tools lib32stdc++6 lib32z1 lib32z1-dev curl wget

RUN /usr/sbin/useradd --create-home --home-dir /srv/insurgency --shell /bin/bash insurgency

RUN mkdir -p /srv/insurgency
ADD files/insserver.tgz /srv/insurgency/
RUN chmod +x /srv/insurgency/insserver
RUN chown -R insurgency /srv/insurgency

ADD https://github.com/kelseyhightower/confd/releases/download/v0.6.3/confd-0.6.3-linux-amd64 /usr/local/bin/confd
RUN chmod +x /usr/local/bin/confd

ADD files/confd-watch /usr/local/bin/confd-watch
RUN chmod +x /usr/local/bin/confd-watch

ADD files/cronfile /etc/cron.d/update_insurgency

ENV HOME /srv/insurgency

USER insurgency
RUN /srv/insurgency/insserver auto-install

CMD ["/usr/local/bin/confd-watch"]
