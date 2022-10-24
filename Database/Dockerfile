# build with: docker build -t ghcr.io/muscle-up-app/database:muscleUpDev -f Database/Dockerfile .
# push with: docker push ghcr.io/muscle-up-app/database:muscleUpDev

FROM postgres:14.5
ENV POSTGRES_PASSWORD=Pa55w0rd
COPY Database/initialization.sql /docker-entrypoint-initdb.d/initialization.sql
