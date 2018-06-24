#!/usr/bin/env python

# Copyright 2015 The Kubernetes Authors All rights reserved.
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

import os
import subprocess
from PIL import Image
from celery import Celery
from os import system,path

# Get Kubernetes-provided address of the broker service
broker_service_host = os.environ.get('GATE_RABBITMQ_SERVICE_HOST')

app = Celery('tasks', broker='amqp://user:G88oWl99U0@%s//' % broker_service_host)

@app.task
def generate_thumbnail(image_name):
  thumbnail = os.path.splitext(image_name)[0] + ".thumb.jpg"

  img = Image.open(image_name)
  wpercent = 160 / float(img.size[0])
  hsize = int(float(img.size[1]) * float(wpercent))
  new_img = img.resize((160, hsize), Image.ANTIALIAS)
  new_img.save(thumbnail)

@app.task
def encode_webm_video(avi_name):
  webm_filename = os.path.splitext(avi_name)[0] + ".webm"

  cmdline = [
    '/usr/bin/nice',
    '-n',
    '15',
    '/usr/bin/avconv',
    '-y',
    '-i',
    avi_name,
    '-codec:v',
    'libvpx',
    '-cpu-used',
    '0',
    '-b:v',
    '500k',
    '-qmin',
    '10',
    '-qmax',
    '42',
    '-maxrate',
    '500k',
    '-bufsize',
    '10000k',
    '-vf',
    'scale=-1:540',
    webm_filename,
  ]

  subprocess.call(cmdline)

@app.task
def encode_ipad_video(avi_name):
  ipad_filename = os.path.splitext(avi_name)[0] + ".ipad.mp4"

  cmdline = [
    '/usr/bin/nice',
    '-n',
    '15',
    '/usr/bin/avconv',
    '-y',
    '-i',
    avi_name,
    '-codec:v',
    'libx264',
    '-pre',
    'slow',
    '-profile:v',
    'baseline',
    '-level',
    '30',
    '-maxrate',
    '500k',
    '-bufsize',
    '10000k',
    '-b:v',
    '500k',
    '-f',
    'mp4',
    '-vf',
    'scale=-1:540',
    ipad_filename,
  ]

  subprocess.call(cmdline)
