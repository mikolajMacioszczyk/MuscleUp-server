# Roles:
- Guest
- Admin
- Worker
- Trainer
- Member
- All = Guest, Admin, Worker, Trainer, Member
- LoggedIn = Admin, Worker, Trainer, Member

# Auth

## Account
| Endpoint Name | Who can | Method type | Purpose |
| ------- | ------------------------------- | ------- | ------------------------------- |
| api/auth/account/login | Guest | POST | logging in to get access and refresh token |
| api/auth/account/login-with-refresh-token | All  | POST | extension of the login validity time, generates new tokens |
| api/auth/account/chanage-password | LoggedIn | POST | changes user password |
| api/auth/account/logout | LoggedIn | PUT | Revocation of access and refresh token before the standard time has elapsed |

## Member
| Endpoint Name | Who can | Method type | Purpose |
| ------- | ------------------------------- | ------- | ------------------------------- |
| api/auth/member/all | Admin | GET | get the data of all Members |
| api/auth/member | Member | GET | get the data of currently logged in Member |
| api/auth/member/by-ids/{userIds}?separator=<string> | LoggedIn | GET | get the data of Members whose id is include in provided list. Default separator = ',' |
| api/auth/member/register | Guest | POST | register Member account |
| api/auth/member | Member | PUT | update data of currently logged in Member |

## Trainer
| Endpoint Name | Who can | Method type | Purpose |
| ------- | ------------------------------- | ------- | ------------------------------- |
| api/auth/trainer/all | Admin | GET | get the data of all Trainers |
| api/auth/trainer | Trainer | GET | get the data of currently logged in Trainer |
| api/auth/trainer/by-ids/{userIds}?separator=<string> | LoggedIn | GET | get the data of Trainers whose id is include in provided list. Default separator = ',' |
| api/auth/trainer/register | Guest | POST | register Trainer account |
| api/auth/trainer | Trainer | PUT | update data of currently logged in Trainer |

## Worker
| Endpoint Name | Who can | Method type | Purpose |
| ------- | ------------------------------- | ------- | ------------------------------- |
| api/auth/worker/all | Admin | GET | get the data of all Workers |
| api/auth/worker | Worker | GET | get the data of currently logged in Worker |
| api/auth/worker/by-ids/{userIds}?separator=<string> | LoggedIn | GET | get the data of Workers whose id is include in provided list. Default separator = ',' |
| api/auth/worker/register | Guest | POST | register Worker account |
| api/auth/worker | Worker | PUT | update data of currently logged in Worker |

# Carnets

## Class Permission
| Endpoint Name | Who can | Method type | Purpose |
| ------- | ------------------------------- | ------- | ------------------------------- |
| api/carnets/class-permission | Worker | GET | returns all ClassPermissions created within the FitnessClub |
| api/carnets/class-permission/all-as-admin/{fitnessClubId} | Admin | GET | returns all ClassPermissions created within the FitnessClub |
| api/carnets/class-permission/{permissionId} | Worker, Admin | GET | returns a single ClassPermission by id |
| api/carnets/class-permission/by-gympass-type/{gympassTypeId} | LoggedIn | GET | returns all ClassPermissions assigned to GympassType indentified by id |
| api/carnets/class-permission | Worker | POST | creates new ClassPermission |
| api/carnets/class-permission/{permissionId} | Worker | DELETE | deletes single ClassPermission |

## Perk Permission
| Endpoint Name | Who can | Method type | Purpose |
| ------- | ------------------------------- | ------- | ------------------------------- |
| api/carnets/perk-permission | Worker | GET | returns all PerkPermissions created within the FitnessClub |
| api/carnets/perk-permission/all-as-admin/{fitnessClubId} | Admin | GET | returns all PerkPermissions created within the FitnessClub |
| api/carnets/perk-permission/{permissionId} | Worker, Admin | GET | returns a single PerkPermission by id |
| api/carnets/class-permission/by-gympass-type/{gympassTypeId} | LoggedIn | GET | returns all PerkPermissions assigned to GympassType indentified by id |
| api/carnets/perk-permission | Worker | POST | creates new PerkPermission |
| api/carnets/perk-permission/{permissionId} | Worker | DELETE | deletes single PerkPermission |

## Gympass
| Endpoint Name | Who can | Method type | Purpose |
| ------- | ------------------------------- | ------- | ------------------------------- |
| api/carnets/gympass | Member, Admin | GET | returns all logged in Member's Gympasses - for Member or all Gympasses in the system - for Admin |
| api/carnets/gympass/from-fitness-club | Worker | GET | returns all Gympasses from FitnessClub |
| api/carnets/gympass/{gympassId} | LoggedIn | GET | returns single Gympass by id |
| api/carnets/gympass | Member | POST | creates Gympass |
| api/carnets/gympass/cancel/{gympassId} | Member | PUT | cancells the Gympass |
| api/carnets/gympass/cancel-as-worker/{gympassId} | Worker | PUT | cancells the Gympass |
| api/carnets/gympass/activate/{gympassId} | Worker | PUT | activates the Gympass |
| api/carnets/gympass/deactivate/{gympassId} | Worker | PUT | deactivates the Gympass |
| api/carnets/gympass/entry/{gympassId} | Member, Worker | PUT | decreases the number of allowed entries by one |

## Gympass Type
| Endpoint Name | Who can | Method type | Purpose |
| ------- | ------------------------------- | ------- | ------------------------------- |
| api/carnets/gympass-type/{gympasstypeId} | LoggedIn | GET | returns single GympassType by id with Permissions |
| api/carnets/gympass-type/active-as-worker?pageNumber=<int>&pageSize=<int> | Worker | GET | returns <pageSize> active GympassTypes with Permissions from FitnessClub |
| api/carnets/gympass-type/active/{fitnessClubId}?pageNumber=<int>&pageSize=<int> | Member, Admin | GET | returns <pageSize> active GympassTypes with Permissions from FitnessClub |
| api/carnets/gympass-type | Worker | POST | creates GympassType and binds ClassPermissions and PerkPermissions. If any of them do not exist, GympassType will not be created  |
| api/carnets/gympass-type/{gympasstypeId} | Worker | PUT | sets an existing GympassType inactive and returns a new, updated and active entity |
| api/carnets/gympass-type/withPermissions/{gympasstypeId} | Worker | PUT | sets an existing GympassType inactive and returns a new, updated and active entity. Links only permissions specified in request body |
| api/carnets/gympass-type/{gympasstypeId} | Worker | DELETE | If there is no associated Gympass, removes GympassType |

## Permission
| Endpoint Name | Who can | Method type | Purpose |
| ------- | ------------------------------- | ------- | ------------------------------- |
| api/carnets/permission/gympass-permissions/{gympassId} | LoggedIn | GET | returns all Permissions associated with GympassType |
| api/carnets/permission/grant | Worker | POST | grants Permission to Gympass |
| api/carnets/permission/revoke | Worker | DELETE | removes single association between Permission and Gympass |
| api/carnets/permission/revoke-all/{permissionId} | Worker | DELETE | removes all associations between Permission and Gympass |

## Subscription
| Endpoint Name | Who can | Method type | Purpose |
| ------- | ------------------------------- | ------- | ------------------------------- |
| api/carnets/subscription/by-gympass/{gympassId} | Member | GET | returns all Subscriptions associated with Gympass |
| api/carnets/subscription/by-gympass-as-worker/{gympassId} | Worker, Admin | GET | returns all Subscriptions associated with Gympass |
| api/carnets/subscription/by-member | Member | GET | returns all Member's Subscriptions |
| api/carnets/subscription/by-member-as-worker | Worker, Admin | GET | returns all Member's Subscriptions |
| api/carnets/subscription/{subscriptionId} | Member, Worker, Admin | GET | returns single Subscription by id |
| api/carnets/subscription | Member | POST | creates Subscription |
| api/carnets/subscription/as-worker | Worker | POST | creates Subscription |

# Content

# Fitness Clubs

## Fitness Club
| Endpoint Name | Who can | Method type | Purpose |
| ------- | ------------------------------- | ------- | ------------------------------- |
| api/fitness-clubs/fitness-club | All | GET | returns all FitnessClubs in application |
| api/fitness-clubs/fitness-club/{fitnessClubId} | All | GET | returns single FitnessClub by id |
| api/fitness-clubs/fitness-club/worker/{workerId} | Worker, Admin | GET | returns FitnessClub of worker |
| api/fitness-clubs/fitness-club | Admin | POST | creates FitnessClub |
| api/fitness-clubs/fitness-club/{fitnessClubId} | Admin | DELETE | deletes FitnessClub |

## Worker Empoyment
| Endpoint Name | Who can | Method type | Purpose |
| ------- | ------------------------------- | ------- | ------------------------------- |
| api/fitness-clubs/worker-employment/{fitnessClubId} | Worker, Admin | GET | returns all WorkerEmployments from FitnessClub |
| api/fitness-clubs/worker-employment | Worker, Admin | POST | creates WorkerEmployment |
| api/fitness-clubs/worker-employment/{workerEmploymentId} | Worker, Admin | PUT | sets an end date for employment, making it inactive |

# Groups

## Group
| Endpoint Name             | Who can   | Method type | Purpose                         |
|---------------------------|-----------|-------------|---------------------------------|
| api/group/find/{id}       | TODO      | GET         | returns single Group by id      |
| api/group/full-group-info | TODO      | GET         | returns list of all Groups      |
| api/group/group-names     | TODO      | GET         | returns list of all Group names |
| api/group/save            | TODO      | POST        | creates Group                   |
| api/group/update          | TODO      | PUT         | updates existing Group          |
| api/group/delete/{id}     | TODO      | DELETE      | cascade deletes Group           |

## GroupWorkout
| Endpoint Name                              | Who can | Method type | Purpose                                                |
|--------------------------------------------|---------|-------------|--------------------------------------------------------|
| api/group-workout/find/{id}                | TODO    | GET         | returns single GroupWorkout by id                      |
| api/group-workout/full-group-workout-info  | TODO    | GET         | returns list of all GroupWorkouts                      |
| api/group-workout/find-by-workout          | TODO    | GET         | returns list of all GroupWorkouts with given workoutId |
| api/group-workout/find-by-group            | TODO    | GET         | returns list of all GroupWorkouts with given groupId   |
| api/group-workout/update                   | TODO    | PUT         | updates existing GroupWorkout                          |
| api/group-workout/delete/{id}              | TODO    | POST        | creates GroupWorkout                                   |
| api/group-workout/delete/{id}              | TODO    | DELETE      | cascade deletes GroupWorkout                           |

## GroupTrainer
| Endpoint Name                                    | Who can | Method type | Purpose                                                   |
|--------------------------------------------------|---------|-------------|-----------------------------------------------------------|
| api/group-trainer/assign                         | TODO    | POST        | assign trainer to group                                   |
| api/group-trainer/unassign/{id}                  | TODO    | DELETE      | delete trainer from group based on groupTrainerId         |
| api/group-trainer/unassign/{trainerId}/{groupId} | TODO    | DELETE      | delete trainer from group based on trainerId and groupId  |

## GroupWorkoutPermission
| Endpoint Name                                                       | Who can | Method type | Purpose                                                                      |
|---------------------------------------------------------------------|---------|-------------|------------------------------------------------------------------------------|
| api/group-workout-permission/add                                    | TODO    | POST        | add permission to groupWorkout                                               |
| api/group-workout-permission/remove/{id}                            | TODO    | DELETE      | remove permission from groupWorkout based on groupWorkoutPermissionId        |
| api/group-workout-permission/remove/{groupWorkoutId}/{permissionId} | TODO    | DELETE      | remove permission from groupWorkout based on groupWorkoutId and permissionId |

## GroupWorkoutParticipant
| Endpoint Name                                                       | Who can  | Method type | Purpose                                                                        |
|---------------------------------------------------------------------|----------|-------------|--------------------------------------------------------------------------------|
| api/group-workout-participant/assign                                | TODO     | POST        | assign user(gympassId) to groupWorkout                                         |
| api/group-workout-participant/unassign/{id}                         | TODO     | DELETE      | delete user(gympassId) from groupWorkout based on groupWorkoutParticipantId    |
| api/group-workout-participant/unassign/{groupWorkoutId}/{gympassId} | TODO     | DELETE      | delete user(gympassId) from groupWorkout based on groupWorkoutId and gympassId |




# Notifications