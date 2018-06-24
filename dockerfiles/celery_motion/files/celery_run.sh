#!/bin/bash

unset CELERY_BROKER_URL
/usr/local/bin/celery -A celery_conf worker
