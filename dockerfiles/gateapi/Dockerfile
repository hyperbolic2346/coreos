FROM node
#FROM node:4.3.1

ADD files/gate_api_swagger /gateapi
ADD files/default.json /gateapi/config

EXPOSE 8080

WORKDIR /gateapi

RUN npm install

CMD ["node", "."]

