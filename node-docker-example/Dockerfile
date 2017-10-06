# Source image - starting point (need a FROM statement at the top of Dockerfile)
FROM ubuntu

RUN apt-get update

# The -y flag ensures we automatically answer "yes" to any prompts like "Do you want to continue?"
RUN apt-get install nodejs -y
RUN apt-get install npm -y

# Copy entire directory into /var/www
RUN mkdir /var/www
ADD . /var/www

# Install node modules
RUN mkdir /var/www/node_modules
RUN npm install --prefix ./var/www

CMD ["nodejs", "/var/www/server.js"]