FROM ubuntu:14.04
RUN apt-get update && apt-get install -yq libmysqlclient18 libmagickwand5 && rm -rf /var/lib/apt/lists/*

ADD https://github.com/kelseyhightower/confd/releases/download/v0.6.3/confd-0.6.3-linux-amd64 /usr/local/bin/confd
RUN chmod +x /usr/local/bin/confd

ADD files/confd-watch /usr/local/bin/confd-watch
RUN chmod +x /usr/local/bin/confd-watch

RUN ln -fs /usr/share/zoneinfo/America/New_York /etc/localtime && dpkg-reconfigure --frontend noninteractive tzdata

CMD ["/usr/local/bin/confd-watch"]
