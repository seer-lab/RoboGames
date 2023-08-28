FROM node:latest
WORKDIR /usr/src/app
ENV PORT=3000
ENV MONGO_DB=robodb
ENV DATABASE_URL=mongodb
ENV DATABASE_PORT=27017
COPY package*.json .
RUN npm ci
COPY . .
CMD ["npm", "start"]