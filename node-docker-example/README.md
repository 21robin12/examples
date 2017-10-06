# Node & Docker

### On Ubuntu

**Docker for Windows uses Hyper-V, which requires Windows Pro edition (£120 upgrade from Windows Home)**

Following this guide: http://www.psychocats.net/ubuntu/virtualbox 

 - Install virtualbox http://www.oracle.com/technetwork/server-storage/virtualbox/downloads/index.html#vbox
 - Create a new VM, select Ubuntu 64-bit, assign RAM & disk space (apparently Fixed is better for perf)
 - Download Ubuntu 64-bit ISO https://www.ubuntu.com/download/desktop
 - In VirtualBox, go Settings / Storage then in Storage Tree click the little CD plus symbol and select the Ubuntu .ISO. Click OK
 - Start the VM - it will open the Ubuntu installer. Run through installation & restart once complete
 - Install docker on Ubuntu using this guide: https://docs.docker.com/engine/installation/linux/docker-ce/ubuntu/

Docker commands:

 - Build with `sudo docker build -t node-docker .`
 - View image with `sudo docker images`
 - Run with `sudo docker run -p 4000:80 node-docker` (this maps port 80 in the container to port 4000 on our machine)
 - View current running containers with `sudo docker container ls`
 - Stop a container with `sudo docker stop <CONTAINER ID>`
 - Run a container in detatched mode using `sudo docker run -d -p 4000:80 node-docker`
 
Got up to https://docs.docker.com/get-started/part2/#share-your-image

### On Windows

Can run Docker on Windows Home using **Docker Toolbox**. Docker staff on Docker Toolbox:

> Legacy desktop solution. Docker Toolbox is for older Mac and Windows systems that do not meet the requirements of Docker for Mac and Docker for Windows. We recommend updating to the newer applications, if possible.

Guide: https://docs.docker.com/toolbox/toolbox_install_windows

 - Download & install Docker Toolbox from https://www.docker.com/products/docker-toolbox (default install settings)
 - Run "Docker Quickstart Terminal" - this sets up Docker & lauches a MinGW command window when complete. Apparently this command window is required to run Docker.
 - From this terminal, navigate to Windows filesystem using e.g. `cd  ~/desktop/Repos/node-docker-example`
 - Build: `docker build -t node-docker .`
 - Run headless: `docker run -d -p 4000:80 node-docker`
 - Visit site: http://192.168.99.100:4000/ (this is the IP of our Linux VM. Can be handy to map to e.g. `docker` using hosts file)
 - Note that containers have to be restarted when the host box (in this case my Windows desktop) restarts

### Useful commands

 - Browse filesystem of running container: `docker exec -t -i d68ed5817b9c /bin/bash` (using ID or name of container)
 - List machines with `docker-machine ls` and get the IP of a machine using `docker-machine ip default`