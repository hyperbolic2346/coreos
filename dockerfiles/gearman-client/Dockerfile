# Gearman
#
FROM ubuntu:14.04
MAINTAINER Mike Wilson <knobby@burntsheep.com>

# update
RUN apt-get update && apt-get install -y php5-cli php5-gearman libav-tools imagemagick

ADD files/create_videos.php /usr/local/bin/create_videos.php

CMD ["/usr/bin/php", "/usr/local/bin/create_videos.php"]
