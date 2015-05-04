# Openhab 1.5.1
# * configuration is injected
#
FROM ubuntu:14.04
MAINTAINER Mike Wilson <knobby@burntsheep.com>

RUN apt-get -y update
RUN apt-get -y upgrade
RUN apt-get -y install unzip wget git

# Download and install Oracle JDK
# For direct download see: http://stackoverflow.com/questions/10268583/how-to-automate-download-and-installation-of-java-jdk-on-linux
RUN wget --no-check-certificate --no-cookies --header "Cookie: oraclelicense=accept-securebackup-cookie" -O /tmp/jdk-7u67-linux-x64.tar.gz http://download.oracle.com/otn-pub/java/jdk/7u67-b01/jdk-7u67-linux-x64.tar.gz
RUN tar -zxC /opt -f /tmp/jdk-7u67-linux-x64.tar.gz
RUN ln -s /opt/jdk1.7.0_67 /opt/jdk7

# Download Openhab 1.7.0
ADD https://openhab.ci.cloudbees.com/job/openHAB/lastSuccessfulBuild/artifact/distribution/target/distribution-1.7.0-SNAPSHOT-runtime.zip /tmp/distribution-1.7.0-runtime.zip
ADD https://openhab.ci.cloudbees.com/job/openHAB/lastSuccessfulBuild/artifact/distribution/target/distribution-1.7.0-SNAPSHOT-addons.zip /tmp/distribution-1.7.0-addons.zip

RUN mkdir -p /opt/openhab/addons-avail
RUN unzip -d /opt/openhab /tmp/distribution-1.7.0-runtime.zip
RUN unzip -d /opt/openhab/addons-avail /tmp/distribution-1.7.0-addons.zip
RUN chmod +x /opt/openhab/start.sh
RUN mkdir -p /opt/openhab/logs

# Add habmin
RUN mkdir -p /opt/openhab/webapps/habmin
RUN git clone https://github.com/cdjackson/HABmin.git /opt/openhab/webapps/habmin

RUN mv /opt/openhab/webapps/habmin/addons/* /opt/openhab/addons-avail

ADD http://downloads.sourceforge.net/project/sigar/sigar/1.6/hyperic-sigar-1.6.4.tar.gz /tmp/hyperic-sigar-1.6.4.tar.gz
RUN mkdir -p /opt/openhab/lib
RUN tar -zxf /tmp/hyperic-sigar-1.6.4.tar.gz --wildcards --strip-components=2 -C /opt/openhab hyperic-sigar-1.6.4/sigar-bin/lib/*

ADD files/boot.sh /usr/local/bin/boot.sh
RUN chmod +x /usr/local/bin/boot.sh

# Clean up
RUN rm -rf /tmp/*

EXPOSE 8080 8443 5555 9001

CMD ["/usr/local/bin/boot.sh"]

