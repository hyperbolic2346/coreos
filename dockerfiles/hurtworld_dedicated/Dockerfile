FROM ubuntu:latest
MAINTAINER Mike Wilson <knobby@burntsheep.com>

RUN apt-get update && apt-get install -yq gdb mailutils postfix lib32gcc1 cron tmux python ca-certificates net-tools lib32stdc++6 lib32z1 lib32z1-dev curl wget unzip

RUN /usr/sbin/useradd --create-home --home-dir /srv/hurtworld --shell /bin/bash hurtworld

RUN mkdir -p /srv/hurtworld
ADD files/hwserver.sh /srv/hurtworld/
RUN chmod +x /srv/hurtworld/hwserver.sh
RUN chown -R hurtworld /srv/hurtworld

ADD files/cronfile /etc/cron.d/update_hurtworld
ADD files/confd-watch /usr/local/bin/confd-watch
RUN chmod +x /usr/local/bin/confd-watch

RUN echo America/New_York > /etc/timezone && dpkg-reconfigure --frontend noninteractive tzdata

ENV HOME /srv/hurtworld

USER hurtworld
RUN /srv/hurtworld/hwserver.sh auto-install

#USER root
#ADD http://www.livemap.io/latest.zip /tmp/latest.zip
#ADD https://github.com/OxideMod/Snapshots/raw/master/Oxide-Hurtworld_Linux.zip /tmp/Oxide-Hurtworld.zip
#ADD files/BetterChat.cs /srv/hurtworld/serverfiles/BetterChat.cs
#ADD files/Teleportation.cs /srv/hurtworld/serverfiles/Teleportation.cs

#RUN unzip -o /tmp/latest.zip -d /srv/hurtworld/serverfiles/
#RUN unzip -o /tmp/Oxide-Hurtworld.zip -d /srv/hurtworld/serverfiles/

#RUN chown -R hurtworld /srv/hurtworld

#USER hurtworld
CMD ["/usr/local/bin/confd-watch"]
