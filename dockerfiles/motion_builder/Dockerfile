apt update && apt install git autoconf automake pkgconf libtool libmysqlclient-dev libjpeg8-dev build-essential libzip-dev ffmpeg libavformat-dev libavcodec-dev libavutil-dev libswscale-dev

RUN git clone https://github.com/hyperbolic2346/motion-1.git

WORKING_DIR /motion-1
RUN autoreconf -fiv
RUN ./configure
RUN make

