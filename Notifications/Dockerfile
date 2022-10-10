# build with: docker build -t ghcr.io/muscle-up-app/notifications:muscleUpDev -f Notifications/Dockerfile .
# push with: docker push ghcr.io/muscle-up-app/notifications:muscleUpDev

FROM gradle:7.5-jdk17-jammy AS TEMP_BUILD_IMAGE
ENV APP_HOME=/app/
WORKDIR $APP_HOME

COPY Notifications/settings.gradle $APP_HOME
COPY Notifications/gradle $APP_HOME/gradle
COPY --chown=gradle:gradle . /home/gradle/src

USER root
RUN chown -R gradle /home/gradle/src
RUN gradle build || return 0

COPY ./Notifications .
RUN gradle clean build

# actual container
FROM openjdk:17-jdk
ENV ARTIFACT_NAME=Notifications-0.0.1-SNAPSHOT.jar
ENV APP_HOME=/app/

WORKDIR $APP_HOME
COPY --from=TEMP_BUILD_IMAGE $APP_HOME/build/libs/$ARTIFACT_NAME .

EXPOSE 8080
ENTRYPOINT exec java -jar ${ARTIFACT_NAME}