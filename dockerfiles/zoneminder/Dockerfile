FROM phusion/baseimage
MAINTAINER Mike Wilson <knobby@burntsheep.com>

# Resynchronize the package index files 
RUN export DEBCONF_NONINTERACTIVE_SEEN=true DEBIAN_FRONTEND=noninteractive && \
	apt-get update && apt-get install -y libvlc5 libcurl3 \
	libdbi-perl libarchive-zip-perl libdate-manip-perl libsys-cpu-perl libdevice-serialport-perl libmime-perl libpcre3 \
	libwww-perl libdbd-mysql-perl libsys-meminfo-perl libsys-mmap-perl libjpeg-turbo8 libav-tools mysql-client \
	apache2 php php7.0-mysql libapache2-mod-php php-cli sudo libtool libmime-lite-perl dh-autoreconf dpatch && \
	apt-get clean -y && \
	rm -rf /var/lib/apt/lists/*

# Copy local code into our container
ADD files/zm.tgz /

RUN mkdir -p /config/data/www /config/data/cgi-bin /etc/my_init.d /content

ADD files/zmlinkcontent.sh /defaults/zmlinkcontent.sh

# ensure writable folders
RUN /defaults/zmlinkcontent.sh -z /defaults/zm.conf /content

# add startup files
ADD files/firstrun.sh /etc/my_init.d/firstrun.sh
RUN chmod +x /etc/my_init.d/firstrun.sh

# give files in /defaults/
RUN chown -R www-data:www-data /defaults/

# Adding apache virtual hosts file
ADD files/apache-vhost /etc/apache2/sites-available/000-default.conf
ADD files/phpdate.ini /etc/php5/apache2/conf.d/25-phpdate.ini

RUN a2enmod cgi rewrite

# Manually set the apache environment variables in order to get apache to work immediately.
RUN \
echo www-data > /etc/container_environment/APACHE_RUN_USER && \
echo www-data > /etc/container_environment/APACHE_RUN_GROUP && \
echo /var/log/apache2 > /etc/container_environment/APACHE_LOG_DIR && \
echo /var/lock/apache2 > /etc/container_environment/APACHE_LOCK_DIR && \
echo /var/run/apache2.pid > /etc/container_environment/APACHE_PID_FILE && \
echo /var/run/apache2 > /etc/container_environment/APACHE_RUN_DIR

# Add apache to runit
RUN mkdir /etc/service/apache
ADD files/apache-run.sh /etc/service/apache/run
RUN chmod +x /etc/service/apache/run
ADD files/apache-finish.sh /etc/service/apache/finish
RUN chmod +x /etc/service/apache/finish

# Add a user we can actually login with
RUN useradd -m -s /bin/bash -G sudo zoneminder
RUN echo 'zoneminder:zoneminder' | chpasswd

RUN ln -s /config/zm.conf /etc/zm.conf
RUN mv /etc/php/7.0/cli/php.ini /defaults/php_cli.ini && mv /etc/php/7.0/apache2/php.ini /defaults/php_apache2.ini
RUN ln -s /config/php/php_cli.ini /etc/php/7.0/cli/php.ini && ln -s /config/php/php_apache2.ini /etc/php/7.0/apache2/php.ini

VOLUME ["/config", "/content"]

# Expose http port
EXPOSE 80
