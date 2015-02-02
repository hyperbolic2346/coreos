# coreos
All my coreos setup: fleet files, dockerfiles

 * fleetctl is a dump of my fleetctl files. These are really systemctl files with some extra markup to tell fleet what services belong where.
 * dockerfiles is a dump of all the dockerfiles I've made for different services on the cluster. This is everything from a private docker repo to a dedicated insurgency game server
 * confd_files is a dump of all the confd configuration and template files I use. I use confd as a way to synchronize all my servers. I really should look into the new things that have come out such as docker links, but I haven't had the chance yet and this way is working well, so why change it?

# A little about my setup
I'm using [coreos](www.coreos.com) to create a cluster of 6 local machines currently. These range in age from months to many years. I'm using some fleet meta tags to indicate which servers are needed for which tasks. Anything processor intensive gets the tag 'speed'. I also have a server with special hardware on it(a zwave dongle and a insteon hub) and I use a meta tag to flag that along with anything that requires that hardware to operate. I've been using [confd](https://github.com/kelseyhightower/confd) to get my services in touch with each other. As an example I have lots of web servers running all around the cluster somewhere, but I don't want to have to figure out the address each time. I use nginx and confd to route requests. Further, I use [upnp](http://http://miniupnp.free.fr/) to route packets to the proper machine. This is the first valuable thing I've seen from upnp. I usually just turn it off due to security concerns, but I can't find a better way to do this work without sticking a single unix machine as a gateway. This allows nginx to bounce around the cluster and hits to my main IP to still make their way to it(and then off to whatever machine on the cluster is running the service requested).

I do still have single points of failure in my network I would like to address. Things like a single fileserver with nfs mounts for most of this content. 

I really like coreos and it has forced me to get into the docker mindset. You can probably see the early containers I created vs the older ones as I have progressed in my thinking. I'd like to find some common ways to handle certain things such as cloud config files that have slight differences between cluster machines and even the systemctl files. Most of those have the same preamble just changed slightly for the service. I think it would be useful to have some include functionality there so I could setup the 'global' stuff once and then just include that file, but I haven't found any such feature yet.


