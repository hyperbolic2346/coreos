# Motion
#
FROM ubuntu
MAINTAINER Mike Wilson <knobby@burntsheep.com>

# update
#RUN apt-get update && apt-get install -y motion ffmpeg libmysqlclient-dev libavformat-dev libavcodec-dev libavutil-dev libswscale-dev python2.7 python-gearman software-properties-common
RUN apt-get update && apt-get install -y motion ffmpeg libjpeg-dev libav-tools python-pip libmysqlclient-dev libavformat-dev libavcodec-dev libavutil-dev libswscale-dev python3-celery software-properties-common

RUN pip install pillow celery

# replace motion with our compiled version
RUN rm /usr/bin/motion
ADD files/motion /usr/bin/motion

RUN mkdir /motion_scripts

ADD files/movie_end /motion_scripts/movie_end
ADD files/photo_saved /motion_scripts/photo_saved
RUN chmod +x /motion_scripts/photo_saved
RUN chmod +x /motion_scripts/movie_end
ADD files/celery_conf.py /motion_scripts/celery_conf.py

ADD files/motion.default.conf /etc/motion/motion.conf
RUN ln -fs /usr/share/zoneinfo/America/New_York /etc/localtime && dpkg-reconfigure --frontend noninteractive tzdata

CMD ["/usr/bin/motion", "-n", "-c", "/etc/motion/motion.conf"]
