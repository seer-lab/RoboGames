FROM node:latest

RUN mkdir -p /usr/src/app
WORKDIR /usr/src/app

RUN npm install -g http-server
COPY . /usr/src/app
EXPOSE 8080

CMD [ "http-server", "-p8080", "--cors='Access-Control-Allow-Origin: *'" ]
