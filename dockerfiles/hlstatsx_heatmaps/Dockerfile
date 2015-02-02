FROM php:5.6-cli
RUN apt-get update && apt-get install -yq libpng12-dev && rm -rf /var/lib/apt/lists/*
WORKDIR /usr/local/heatmaps
RUN /usr/local/bin/docker-php-ext-install gd
RUN /usr/local/bin/docker-php-ext-install mysql
CMD [ "php", "./generate.php" ]
