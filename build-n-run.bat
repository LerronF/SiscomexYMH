docker container rm siscomex -f
docker image rm siscomex
docker build . --tag siscomex
docker container run -d --name siscomex siscomex