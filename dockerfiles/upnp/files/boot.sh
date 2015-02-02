#!/bin/bash

while true
do
  # Check the port. We do this in another script due to a memory leak.
  # If we don't do it this way we eventually crash out of memory.
  # Running the script in another process should prevent that.
  /bin/bash /usr/local/bin/poll.sh

  sleep 30
done
