# Gate web page
#
FROM tutum/apache-php
MAINTAINER Mike Wilson <knobby@burntsheep.com>

#RUN apt-get update && apt-get install -yq git && rm -rf /var/lib/apt/lists/*
RUN rm -fr /app /var/www/html
ADD files/gate /var/www
RUN ln -s /var/www/docs /var/www/html

#ADD https://github.com/kelseyhightower/confd/releases/download/v0.6.3/confd-0.6.3-linux-amd64 /usr/local/bin/confd
#RUN chmod +x /usr/local/bin/confd

#RUN mkdir -p /etc/confd/conf.d /etc/confd/templates

#ADD files/gate.toml /etc/confd/conf.d/gate.toml
#ADD files/gate.tmpl /etc/confd/templates/gate.tmpl

ADD files/confd-watch /usr/local/bin/confd-watch
RUN chmod +x /usr/local/bin/confd-watch

RUN ln -fs /usr/share/zoneinfo/America/New_York /etc/localtime && dpkg-reconfigure --frontend noninteractive tzdata

# smash directory listings...sigh
RUN sed -i 's@Options Indexes@Options@' /etc/apache2/apache2.conf

CMD ["/usr/local/bin/confd-watch"]
