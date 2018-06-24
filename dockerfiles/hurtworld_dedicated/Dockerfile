FROM ubuntu:latest
MAINTAINER Mike Wilson <knobby@burntsheep.com>

# If the server is updating when start with docker start
ENV UPDATEONSTART 1

# Install things we need - special hoops to get tmux 2.0
RUN dpkg --add-architecture i386 &&\
    apt-get update &&\
    apt-get install -yq git tmux lib32gcc1 cron curl file lib32gcc1 ca-certificates lib32stdc++6 libstdc++6:i386 lib32z1 lib32z1-dev vim

# Enable passwordless sudo for users under the "sudo" group
#RUN sed -i.bkp -e \
#	's/%sudo\s\+ALL=(ALL\(:ALL\)\?)\s\+ALL/%sudo ALL=NOPASSWD:ALL/g' /etc/sudoers \
#	/etc/sudoers

# Create hurtworld user
RUN /usr/sbin/useradd --create-home --home-dir /hurtworld --shell /bin/bash hurtworld

# Add to sudo group
RUN usermod -a -G sudo hurtworld

RUN mkdir -p /hurtworld/serverfiles
RUN mkdir -p /defaults

COPY files/crontab /defaults/crontab

#RUN git clone -b master https://github.com/dgibbs64/linuxgsm.git /linuxgsm
RUN mkdir /linuxgsm
ADD https://gameservermanagers.com/dl/hwserver /linuxgsm/hwserver
RUN ln -s /linuxgsm/hwserver /usr/local/bin/hwserver &&\
    ln -s /linuxgsm/hwserver /hurtworld/hwserver &&\
    ln -s /linuxgsm/cfg/lgsm-default.cfg /hurtworld/lgsm-default.cfg

#RUN ln -s /linuxgsm/Hurtworld/hwserver /usr/local/bin/hwserver &&\
#    ln -s /linuxgsm/Hurtworld/hwserver /hurtworld/hwserver &&\
#    ln -s /linuxgsm/Hurtworld/cfg/lgsm-default.cfg /hurtworld/lgsm-default.cfg

# change server directory in script
RUN sed -i.bkp -e \
        's/filesdir=\"${rootdir}\/serverfiles\"/filesdir=\"\/hurtworld\/serverfiles\"/g' /linuxgsm/hwserver \
        /linuxgsm/hwserver
#RUN sed -i.bkp -e \
#        's/filesdir=\"${rootdir}\/serverfiles\"/filesdir=\"\/hurtworld\/serverfiles\"/g' /linuxgsm/Hurtworld/hwserver \
#        /linuxgsm/Hurtworld/hwserver


WORKDIR /hurtworld

# useful symlinks
RUN ln -s /linuxgsm/log log

RUN chmod +x /linuxgsm/hwserver
RUN chown -R hurtworld /hurtworld && chown -R hurtworld /linuxgsm && chown -R hurtworld /defaults

ADD files/run.sh /usr/local/bin/run.sh
RUN chmod +x /usr/local/bin/run.sh

# Set the timezone
RUN ln -fs /usr/share/zoneinfo/America/New_York /etc/localtime && dpkg-reconfigure --frontend noninteractive tzdata

ENV HOME /hurtworld

USER hurtworld

ENV LD_LIBRARY_PATH="/hurtworld/serverfiles/bin"

CMD ["/usr/local/bin/run.sh"]
