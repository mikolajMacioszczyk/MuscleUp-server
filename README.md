# Muscle Up Server

## Project Description

The "Muscle Up" project is an application aimed at club members of sports centers, their owners, employees and trainers. 
The list of requirements defined by the project is as follows:
- Each user has the ability to log in online to their account. The exception is a non-logged-in user, who can register.
- Each user can be assigned to more than one club.
- The option to open new clubs and close already open ones, resulting from the possibility of the owner having more than one club.
- Providing an option to manage the types of passes that can be purchased within the created club.
- Enabling club employees to edit passes manually.
- The ability to perform through the application: purchase a pass (membership) online, cancel a pass, or terminate a subscription.
- The club's schedule in the form of a weekly plan, available to logged-in users, which is updated live, so it always provides up-to-date information.
- Each class provides information such as the time, location, number of people who can sign up, who is leading the class and what kind of training will be done at the class.
- Class enrollment system. The clubber, after checking the details of the activities he is interested in, can sign up for them through the app.
- A logged-in clubber can see in his account the upcoming workouts at the club and the activity status of his passes.
- Admission to a given fitness club is to be accessed via a generated QR code.
- The ability to manage both workouts and individual exercises that are available at the selected fitness club, including a system for programming the workouts that will be performed as part of the class.
- A workout can consist of many of the same exercises. 

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

1.  Go to project root folder
2.  Run: 
    - `docker-compose up` to use containers from registry  
    - `docker-compose -f docker-compose.build.yml up --build` command to build and run all services
3.  (Optional, if you want to debug locally) Run interestning project - it should already be configured by default to work with other microservices.  

See the <b>"default_development_data.md"</b> file for the default data available in the database.  

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
