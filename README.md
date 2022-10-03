# Muscle Up Server

## Project structure
* apiGateway - Service that works as an api entrypoint. Distributes packages to the appropriate services based on path mapping. The swagger running at url "{{Base Address}}/swagger/index.html" contains the configuration of all api endpoints.
* auth - User Managment service. Works under the path "{{Base Address}}/auth/...". Consists of 3 sub-projects:
    * API - Api built with MVC controllers 
    * Repo - Data access abstraction
    * Domain - Domain models and interfaces
* carnets - Service tht manages gympasses and their permissions. Works under the path "{{Base Address}}/carets/...". Consists of 3 sub-projects:
    * API - Api built with MVC controllers 
    * Repo - Data access abstraction
    * Domain - Domain models and interfaces
* content - Service that enables access and management of workouts and support materials. Works under the path "{{Base Address}}/content/...".
* fitnessClubs - Service that focuses on the clubs, their employees and trainers. Works under the path "{{Base Address}}/fitnessClubs/...". Consists of 3 sub-projects:
    * API - Api built with MVC controllers
    * Repo - Data access abstraction
    * Domain - Domain models and interfaces
* groups - Service that manages class groups. Works under the path "{{Base Address}}/groups/...".
* notifications - Application notification system. Works under the path "{{Base Address}}/notifications/...".

## Installation & development

### Install

docker client must be installed and running on host machine

### Development

In order to start the development:

1. Go to project root folder
2. Run docker-compose -f docker-compose.build.yml up --build command to build and run all services
3. (Optional, if you want to debug locally) Run interestning project - it should already be configured by default to work with other microservices.

## Deployment

### Environments

| Name    | Base Address (https)            | Base Address (http)            |
| ------- | ------------------------------- |------------------------------- |
| production |     |     |
| staging |  |  |
| dev | https://localhost:8082 | http://localhost:8080 |

Merging a pull request into `staging` or `dev` environment will run a deploy github action.

### Production deployment

Before production deployment prepare the release on Github.

Steps for creating a release on Github:

1. Go to `/releases`.
2. Click `Create a new release`.
3. Create new tag using versioning template vx.y.z which should be next version of the app.
4. Insert Release - vx.y.z title.
5. Describe changes.
6. Publish release.