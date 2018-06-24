FROM ubuntu:trusty
MAINTAINER Mike Wilson <knobby@burntsheep.com>

# If the server is updating when start with docker start
ENV UPDATEONSTART 1

# Install things we need - special hoops to get tmux 2.0
RUN dpkg --add-architecture i386 &&\
    apt-get update &&\
    apt-get install -yq git bsdmainutils tmux lib32gcc1 cron curl file lib32gcc1 ca-certificates lib32stdc++6 libstdc++6:i386 lib32z1 lib32z1-dev vim wget python bzip2 &&\
    apt-get clean -y && \
    rm -rf /var/lib/apt/lists/*

# Enable passwordless sudo for users under the "sudo" group
#RUN sed -i.bkp -e \
#	's/%sudo\s\+ALL=(ALL\(:ALL\)\?)\s\+ALL/%sudo ALL=NOPASSWD:ALL/g' /etc/sudoers \
#	/etc/sudoers

# Create insurgency user
RUN /usr/sbin/useradd --create-home --home-dir /insurgency --shell /bin/bash insurgency

# Add to sudo group
RUN usermod -a -G sudo insurgency

RUN mkdir -p /insurgency/serverfiles
RUN mkdir -p /defaults

COPY files/crontab /defaults/crontab

#RUN git clone -b master https://github.com/dgibbs64/linuxgsm.git /linuxgsm
RUN mkdir /linuxgsm
ADD https://gameservermanagers.com/dl/insserver /linuxgsm/insserver
RUN mv /linuxgsm/insserver /defaults/insserver &&\
    ln -s /linuxgsm/insserver /usr/local/bin/insserver &&\
    ln -s /insurgency/insserver /linuxgsm/insserver

# get our addons
ADD http://cdn.probablyaserver.com/sourcemod/mmsource-1.10.6-linux.tar.gz /defaults/mmsource-1.10.6-linux.tar.gz
RUN tar -xzf /defaults/mmsource-1.10.6-linux.tar.gz -C /defaults/ && rm /defaults/mmsource-1.10.6-linux.tar.gz
RUN git clone -b master https://github.com/jaredballou/insurgency-sourcemod.git /defaults/addons/sourcemod

RUN mkdir -p /insurgency/serverfiles/insurgency/addons && cp -r /defaults/addons/* /insurgency/serverfiles/insurgency/addons/

# change server directory in script
RUN sed -i.bkp -e \
        's/filesdir=\"${rootdir}\/serverfiles\"/filesdir=\"\/insurgency\/serverfiles\"/g' /defaults/insserver \
        /defaults/insserver

# change script directory in script so it is the link dir(/linuxgsm) and doesn't clog up our volume 
RUN sed -i.bkp -e \
        's/rootdir=.*/rootdir=\"\/linuxgsm\/\"/g' /defaults/insserver \
        /defaults/insserver

# install steamcmd manually since we might not run install... sigh
RUN curl -o sc.tgz http://media.steampowered.com/client/steamcmd_linux.tar.gz &&\
    mkdir -p /linuxgsm/steamcmd &&\
    tar -xzf sc.tgz -C /linuxgsm/steamcmd &&\
    rm sc.tgz

WORKDIR /insurgency

# useful symlinks
RUN ln -s serverfiles/insurgency/addons addons &&\
    ln -s serverfiles/insurgency/mapcycle_cooperative.txt mapcycle_cooperative.txt &&\
    ln -s serverfiles/insurgency/subscribed_file_ids.txt subscribed_file_ids.txt &&\
    ln -s serverfiles/insurgency/webapi_authkey.txt webapi_authkey.txt &&\
    ln -s /linuxgsm/log log &&\
    ln -s serverfiles/insurgency/cfg cfg

RUN chmod +x /defaults/insserver
RUN chown -R insurgency /insurgency && chown -R insurgency /linuxgsm && chown -R insurgency /defaults

ADD files/run.sh /usr/local/bin/run.sh
RUN chmod +x /usr/local/bin/run.sh

# Set the timezone
RUN ln -fs /usr/share/zoneinfo/America/New_York /etc/localtime && dpkg-reconfigure --frontend noninteractive tzdata

ENV HOME /insurgency

USER insurgency

ENV LD_LIBRARY_PATH="/insurgency/serverfiles/bin"

CMD ["/usr/local/bin/run.sh"]
