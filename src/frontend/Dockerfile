# Stage build
FROM node:20 AS build
WORKDIR /react-app
COPY package*.json .
#COPY check-node-version.js .
RUN npm i
COPY . .
RUN npm run build

# Stage run
FROM nginx:1.25.5
WORKDIR /usr/share/nginx/html
COPY --from=build /react-app/dist .
COPY ./nginx.conf /etc/nginx/nginx.conf
ENTRYPOINT ["nginx", "-g", "daemon off;"]
