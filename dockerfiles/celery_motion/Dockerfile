# Copyright 2016 The Kubernetes Authors All rights reserved.
#
# Licensed under the Apache License, Version 2.0 (the "License");
# you may not use this file except in compliance with the License.
# You may obtain a copy of the License at
#
#     http://www.apache.org/licenses/LICENSE-2.0
#
# Unless required by applicable law or agreed to in writing, software
# distributed under the License is distributed on an "AS IS" BASIS,
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
# See the License for the specific language governing permissions and
# limitations under the License.

FROM library/celery

USER root

RUN apt-get update && apt-get install -y libav-tools libjpeg-dev && apt-get clean -y && rm -rf /var/lib/apt/lists/*

RUN pip install pillow

RUN ln -s /lib/x86_64-linux-gnu/libz.so.1 /lib/ && ln -s /usr/lib/x86_64-linux-gnu/libfreetype.so.6 /usr/lib/ && ln -s /usr/lib/x86_64-linux-gnu/libjpeg.so.62 /usr/lib/

ADD files/celery_conf.py /data/celery_conf.py
ADD files/celery_run.sh /data/celery_run.sh
RUN chmod a+x /data/celery_run.sh

ENV C_FORCE_ROOT 1

WORKDIR /data
#CMD ["/usr/local/bin/celery", "-A", "celery_conf", "worker"]
CMD ["/data/celery_run.sh"]
