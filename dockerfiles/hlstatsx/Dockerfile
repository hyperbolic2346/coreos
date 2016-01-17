FROM eboraas/apache-php

# fix for above image
RUN a2enmod socache_shmcb

RUN apt-get update && apt-get -y install php5-mysql && apt-get clean

ADD files/hlstatsx.tgz /var/www/html/
RUN chown -R 33 /var/www/html
RUN rm /var/www/html/index.html
