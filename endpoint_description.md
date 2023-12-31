# Roles:
- Guest
- Admin
- Worker
- Trainer
- Member
- Owner (is also Worker)
- All = Guest, Admin, Worker, Trainer, Member, Owner
- LoggedIn = Admin, Worker, Trainer, Member, Owner

# Auth

## Account
| Endpoint Name | Who can | Method type | Purpose |
| ------- | ------------------------------- | ------- | ------------------------------- |
| api/auth/account/login | Guest | POST | logging in to get access and refresh token |
| api/auth/account/login-with-facebook | Guest | POST | logging in using facebook. If user with given email does not exists, creates one (Member) |
| api/auth/account/login-with-refresh-token | All  | POST | extension of the login validity time, generates new tokens |
| api/auth/account/chanage-password | LoggedIn | POST | changes user password |
| api/auth/account/logout | LoggedIn | PUT | Revocation of access and refresh token before the standard time has elapsed |

# User
| Endpoint Name | Who can | Method type | Purpose |
| ------- | ------------------------------- | ------- | ------------------------------- |
| api/auth/user/{email} | LoggedIn | GET | get the data of user by email |
| api/auth/user/by-id/{userId} | LoggedIn | GET | get the data of user by id |

## Member
| Endpoint Name | Who can | Method type | Purpose |
| ------- | ------------------------------- | ------- | ------------------------------- |
| api/auth/member/all | Admin | GET | get the data of all Members |
| api/auth/member | Member | GET | get the data of currently logged in Member |
| api/auth/member/find/{userId} | LoggedIn | GET | get the data of Member by id |
| api/auth/member/by-ids/{userIds}?separator=<string> | LoggedIn | GET | get the data of Members whose id is include in provided list. Default separator = ',' |
| api/auth/member/register | Guest | POST | register Member account |
| api/auth/member | Member | PUT | update data of currently logged in Member |

## Trainer
| Endpoint Name | Who can | Method type | Purpose |
| ------- | ------------------------------- | ------- | ------------------------------- |
| api/auth/trainer/all | Admin | GET | get the data of all Trainers |
| api/auth/trainer | Trainer | GET | get the data of currently logged in Trainer |
| api/auth/trainer/find/{userId} | LoggedIn | GET | get the data of Trainer by id |
| api/auth/trainer/by-ids/{userIds}?separator=<string> | LoggedIn | GET | get the data of Trainers whose id is include in provided list. Default separator = ',' |
| api/auth/trainer/register | Guest | POST | register Trainer account |
| api/auth/trainer | Trainer | PUT | update data of currently logged in Trainer |

## Worker
| Endpoint Name | Who can | Method type | Purpose |
| ------- | ------------------------------- | ------- | ------------------------------- |
| api/auth/worker/all | Admin | GET | get the data of all Workers |
| api/auth/worker | Worker | GET | get the data of currently logged in Worker |
| api/auth/worker/find/{userId} | LoggedIn | GET | get the data of Worker by id |
| api/auth/worker/by-ids/{userIds}?separator=<string> | LoggedIn | GET | get the data of Workers whose id is include in provided list. Default separator = ',' |
| api/auth/worker/register | Guest | POST | register Worker account |
| api/auth/worker | Worker | PUT | update data of currently logged in Worker |
| api/auth/worker/{workerId} | Worker | PUT | update data Worker by id |

## Owner
| Endpoint Name | Who can | Method type | Purpose |
| ------- | ------------------------------- | ------- | ------------------------------- |
| api/auth/owner/all | Admin | GET | get the data of all Owners |
| api/auth/owner | Owner | GET | get the data of currently logged in Owner |
| api/auth/owner/find/{userId} | Owner, Admin | GET | get the data of Owner by id |
| api/auth/owner/register | Guest | POST | register Owner account |
| api/auth/owner | Owner | PUT | update data of currently logged in Owner |

# Carnets

## Class Permission
| Endpoint Name | Who can | Method type | Purpose |
| ------- | ------------------------------- | ------- | ------------------------------- |
| api/carnets/class-permission | Worker | GET | returns all ClassPermissions created within the FitnessClub |
| api/carnets/class-permission/all-from-fitness-club/{fitnessClubId} | Admin, Member | GET | returns all ClassPermissions created within the FitnessClub |
| api/carnets/class-permission/{permissionId} | Worker, Admin | GET | returns a single ClassPermission by id |
| api/carnets/class-permission/by-gympass-type/{gympassTypeId} | LoggedIn | GET | returns all ClassPermissions assigned to GympassType indentified by id |
| api/carnets/class-permission | Worker | POST | creates new ClassPermission |
| api/carnets/class-permission/{permissionId} | Worker | DELETE | deletes single ClassPermission |

## Perk Permission
| Endpoint Name | Who can | Method type | Purpose |
| ------- | ------------------------------- | ------- | ------------------------------- |
| api/carnets/perk-permission | Worker | GET | returns all PerkPermissions created within the FitnessClub |
| api/carnets/perk-permission/all-from-fitness-club/{fitnessClubId} | Admin, Member | GET | returns all PerkPermissions created within the FitnessClub |
| api/carnets/perk-permission/{permissionId} | Worker, Admin | GET | returns a single PerkPermission by id |
| api/carnets/class-permission/by-gympass-type/{gympassTypeId} | LoggedIn | GET | returns all PerkPermissions assigned to GympassType indentified by id |
| api/carnets/perk-permission | Worker | POST | creates new PerkPermission |
| api/carnets/perk-permission/{permissionId} | Worker | DELETE | deletes single PerkPermission |

## Gympass
| Endpoint Name | Who can | Method type | Purpose |
| ------- | ------------------------------- | ------- | ------------------------------- |
| api/carnets/gympass | Member, Admin | GET | returns all logged in Member's Gympasses - for Member or all Gympasses in the system - for Admin |
| api/carnets/gympass/by-member/{memberId} | Worker, Admin | GET | returns all Member's Gympasses |
| api/carnets/gympass/from-fitness-club | Worker | GET | returns all Gympasses from FitnessClub |
| api/carnets/gympass/{gympassId} | LoggedIn | GET | returns single Gympass by id |
| api/carnets/gympass/has-active/{memberId}/{fitnessClubId} | LoggedIn | GET | verifies if Member has active gympass in Fitness Club |
| api/carnets/gympass | Member | POST | creates Gympass |
| api/carnets/gympass/activate/{gympassId} | Worker | PUT | activates the Gympass |
| api/carnets/gympass/deactivate/{gympassId} | Worker | PUT | deactivates the Gympass |

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
| api/carnets/subscription/by-gympass/{gympassId} | Worker, Admin, Member | GET | returns all Subscriptions associated with Gympass |
| api/carnets/subscription/by-member | Member | GET | returns all Member's Subscriptions |
| api/carnets/subscription/by-member-as-worker | Worker, Admin | GET | returns all Member's Subscriptions |
| api/carnets/subscription/{subscriptionId} | Member, Worker, Admin | GET | returns single Subscription by id |
| api/carnets/subscription | Member | POST | creates Subscription |
| api/carnets/subscription/as-worker | Worker | POST | creates Subscription |
| api/carnets/subscription/cancel/{subscriptionId} | Member, Worker | PUT | cancels Subscription both on api and Stripe |
| api/carnets/subscription/cancel-by-gympass/{gympassId} | Member, Worker | PUT | cancels all Gympass Subscriptions both on api and Stripe |

## Entries
| Endpoint Name | Who can | Method type | Purpose |
| ------- | ------------------------------- | ------- | ------------------------------- |
| api/carnets/entry/{entryId} | LoggedIn | GET | returns Entry by id |
| api/carnets/entry/by-gympass/{gympassId}?pageNumber=<int>&pageSize=<int> | Member, Worker, Admin | GET | returns Entries assigned to gympass |
| api/carnets/entry/by-member?pageNumber=<int>&pageSize=<int> | Member | GET | returns Member's Entries |
| api/carnets/entry/by-member/{memberId}?pageNumber=<int>&pageSize=<int> | Worker, Admin | GET | returns Member's Entries |
| api/carnets/entry/generate-token | Member | POST | returns entry token |
| api/carnets/entry | Worker | PUT | creates entry based on entry token |

# Fitness Clubs

## Fitness Club
| Endpoint Name | Who can | Method type | Purpose |
| ------- | ------------------------------- | ------- | ------------------------------- |
| api/fitness-clubs/fitness-club | All | GET | returns all User's FitnessClubs |
| api/fitness-clubs/fitness-club/all | All | GET | returns all FitnessClubs in application |
| api/fitness-clubs/fitness-club/{fitnessClubId} | All | GET | returns single FitnessClub by id |
| api/fitness-clubs/fitness-club/worker/{workerId} | Worker, Admin | GET | returns FitnessClub of worker |
| api/fitness-clubs/fitness-club | Admin | POST | creates FitnessClub |
| api/fitness-clubs/fitness-club/{fitnessClubId} | Admin | DELETE | deletes FitnessClub |

## Worker Empoyment
| Endpoint Name | Who can | Method type | Purpose |
| ------- | ------------------------------- | ------- | ------------------------------- |
| api/fitness-clubs/worker-employment/{fitnessClubId} | Worker, Admin | GET | returns all Workers from FitnessClub |
| api/fitness-clubs/worker-employment/details/{fitnessClubId} | Worker, Admin | GET | returns all WorkerEmployments with Worker's data from FitnessClub |
| api/fitness-clubs/worker-employment | Owner, Admin | POST | creates WorkerEmployment |
| api/fitness-clubs/worker-employment/worker-invitation | Owner, Admin | POST | creates worker invitation and send email to provided address |
| api/fitness-clubs/worker-employment/{workerEmploymentId} | Owner, Admin | PUT | sets an end date for employment, making it inactive |
| api/fitness-clubs/worker-employment/worker-invitation | Worker | PUT | creates worker employment based on invitation token |

## Trainer Empoyment
| Endpoint Name | Who can | Method type | Purpose |
| ------- | ------------------------------- | ------- | ------------------------------- |
| api/fitness-clubs/trainer-employment/{fitnessClubId} | Trainer, Worker, Admin | GET | returns all Trainer from FitnessClub |
| api/fitness-clubs/trainer-employment/details/{fitnessClubId} | Worker, Admin | GET | returns all TrainerEmployments with Trainer's data from FitnessClub |
| api/fitness-clubs/trainer-employment | Worker, Admin | POST | creates TrainerEmployment |
| api/fitness-clubs/trainer-employment/trainer-invitation | Worker, Admin | POST | creates trainer invitation and send email to provided address |
| api/fitness-clubs/trainer-employment/{trainerEmploymentId} | Worker, Admin | PUT | sets an end date for employment, making it inactive |
| api/fitness-clubs/worker-employment/trainer-invitation | Trainer | PUT | creates trainer employment based on invitation token |

## Membership
| Endpoint Name | Who can | Method type | Purpose |
| ------- | ------------------------------- | ------- | ------------------------------- |
| api/fitness-clubs/membership/{membershipId} | Worker | GET | returns Membership with Member's data by Id from Worker's FitnessClub |
| api/fitness-clubs/membership/{membershipId}/{fitnessClubId} | Admin | GET | returns Membership with Member's data by Id from FitnessClub |
| api/fitness-clubs/membership/from-fitness-club | Worker | GET | returns all Memberships with Member's datas from Worker's FitnessClub |
| api/fitness-clubs/membership/from-fitness-club/{fitnessClubId} | Admin | GET | returns all Memberships with Member's datas from FitnessClub |
| api/fitness-clubs/membership | Admin, Worker, Member | POST | creates Membership |

# Groups

## Group
| Endpoint Name  | Who can   | Method type | Purpose                         |
|----------------|-----------|-------------|---------------------------------|
| api/group/{id} | TODO      | GET         | returns single Group by id      |
| api/group/all  | TODO      | GET         | returns list of all Groups      |
| api/group      | TODO      | POST        | creates Group                   |
| api/group/{id} | TODO      | PUT         | updates existing Group          |
| api/group/{id} | TODO      | DELETE      | cascade deletes Group           |

## GroupWorkout
| Endpoint Name          | Who can | Method type | Purpose                           |
|------------------------|---------|-------------|-----------------------------------|
| api/group-workout/{id} | TODO    | GET         | returns single GroupWorkout by id |
| api/group-workout/all  | TODO    | GET         | returns list of all GroupWorkouts |
| api/group-workout      | TODO    | POST        | crates GroupWorkout               |
| api/group-workout/{id} | TODO    | PUT         | updates GroupWorkout              |
| api/group-workout/{id} | TODO    | DELETE      | cascade deletes GroupWorkout      |

## Schedule
| Endpoint Name            | Who can | Method type | Purpose                                                     |
|--------------------------|---------|-------------|-------------------------------------------------------------|
| api/schedule/{id}        | TODO    | GET         | returns aggregated info about single schedule cell          |
| api/schedule/clones/{id} | TODO    | GET         | returns aggregated info about copies of given schedule cell |
| api/schedule/all         | TODO    | GET         | returns aggregated info about all schedule cells            |
| api/schedule/            | TODO    | POST        | create new groups and groupWorkouts                         |
| api/schedule/clones/{id} | TODO    | POST        | pin given groupWorkout data to group as copy                |
| api/schedule/{id}        | TODO    | PUT         | updates groups and groupWorkouts                            |
| api/schedule/{id}        | TODO    | DELETE      | cascade deletes groupWorkout                                |

## GroupWorkoutPermission
| Endpoint Name                                         | Who can | Method type | Purpose                                |
|-------------------------------------------------------|---------|-------------|----------------------------------------|
| api/group-permission/assign                           | TODO    | POST        | assigns permission to groupWorkout     |
| api/group-permission/unassign                         | TODO    | PUT         | unassigns permission from groupWorkout |

## GroupWorkoutParticipant
| Endpoint Name                                                | Who can  | Method type | Purpose                          |
|--------------------------------------------------------------|----------|-------------|----------------------------------|
| api/group-workout-participant/assign                         | TODO     | POST        | assigns user to groupWorkout     |
| api/group-workout-participant/unassign                       | TODO     | PUT         | unassigns user from groupWorkout |


# Content

## BodyPart
| Endpoint Name       | Who can | Method type | Purpose                       |
|---------------------|---------|-------------|-------------------------------|
| api/body-part/{id}  | TODO    | GET         | returns single BodyPart by id |
| api/body-part/all   | TODO    | GET         | return all BodyParts          |
| api/body-part       | TODO    | POST        | saves new BodyPart            |
| api/body-part/{id}  | TODO    | PUT         | updates BodyPart              |
| api/body-part/{id}  | TODO    | DELETE      | deletes BodyPart              |


## ExerciseCriterion
| Endpoint Name                 | Who can | Method type | Purpose                        |
|-------------------------------|---------|-------------|--------------------------------|
| api/criterion/{id}            | TODO    | GET         | returns single Criterion by id |
| api/criterion/all             | TODO    | GET         | returns all Criteria           |
| api/criterion/all-active      | TODO    | GET         | returns all active Criteria    |
| api/criterion                 | TODO    | POST        | creates Criterion              |
| api/criterion/activate/{id}   | TODO    | PUT         | activates Criterion            |
| api/criterion/deactivate/{id} | TODO    | PUT         | deactivates Criterion          |

## Exercise
| Endpoint Name           | Who can | Method type | Purpose                                        |
|-------------------------|---------|-------------|------------------------------------------------|
| api/exercise/{id}       | TODO    | GET         | returns single Exercise by id                  |
| api/exercise/all        | TODO    | GET         | returns all Exercises                          |
| api/exercise/all-active | TODO    | GET         | returns all active Exercises                   |
| api/exercise            | TODO    | POST        | creates Exercise                               |
| api/exercise            | TODO    | PUT         | deactivate orginal Exercise and replace it     |
| api/exercise/{id}       | TODO    | DELETE      | deactivates Exercise or deleteted if is unused |

## PerformedWorkout
| Endpoint Name                        | Who can | Method type | Purpose                                    |
|--------------------------------------|---------|-------------|--------------------------------------------|
| api/performed-workout/all            | TODO    | GET         | returns all PerformedWorkouts              |
| api/performed-workout/performer/{id} | TODO    | GET         | returns all PerformedWorkouts by performer |
| api/performed-workout/creator/{id}   | TODO    | GET         | returns all PerformedWorkouts by creator   |
| api/performed-workout                | TODO    | POST        | creates PerformedWorkout                   |

## Workout
| Endpoint Name                     | Who can | Method type | Purpose                                                                            |
|-----------------------------------|---------|-------------|------------------------------------------------------------------------------------|
| api/workout/{id}                  | TODO    | GET         | returns single Workout by id                                                       |
| api/workout/all                   | TODO    | GET         | return all Workouts                                                                |
| api/workout/most-popular/{pieces} | TODO    | GET         | return given ammount of most performed workout                                     |
| api/workout                       | TODO    | POST        | saves new Workout                                                                  |
| api/workout/{id}                  | TODO    | PUT         | updates Workout. Creator can update description and video, others creates own copy |
| api/workout/{id}                  | TODO    | DELETE      | deletes Workout if nobody performed it                                             |

## Workout
| Endpoint Name                     | Who can | Method type | Purpose                                        |
|-----------------------------------|---------|-------------|------------------------------------------------|
| api/workout/{id}                  | TODO    | GET         | returns single Workout by id                   |
| api/workout/all                   | TODO    | GET         | return all Workouts                            |
| api/workout/most-popular/{pieces} | TODO    | GET         | return given ammount of most performed workout |
| api/workout                       | TODO    | POST        | saves new Workout                              |
| api/workout/{id}                  | TODO    | PUT         | updates Workout                                |
| api/workout/{id}                  | TODO    | DELETE      | deletes Workout                                |

# Notifications