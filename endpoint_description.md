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
| api/auth/account/logout | LoggedIn | GET | Revocation of access and refresh token before the standard time has elapsed |
| api/auth/account/login | Guest | POST | logging in to get access and refresh token |
| api/auth/account/login-with-refresh-token | LoggedIn  | POST | extension of the login validity time, generates new tokens |
| api/auth/account/chanage-password | LoggedIn | POST | changes user password |

## Member
| Endpoint Name | Who can | Method type | Purpose |
| ------- | ------------------------------- | ------- | ------------------------------- |
| api/auth/member/all | Admin | GET | get the data of all Members |
| api/auth/member | Member | GET | get the data of currently logged in Member |
| api/auth/member/register | Guest | POST | register Member account |
| api/auth/member | Member | PUT | update data of currently logged in Member |

## Trainer
| Endpoint Name | Who can | Method type | Purpose |
| ------- | ------------------------------- | ------- | ------------------------------- |
| api/auth/trainer/all | Admin | GET | get the data of all Trainers |
| api/auth/trainer | Trainer | GET | get the data of currently logged in Trainer |
| api/auth/trainer/register | Guest | POST | register Trainer account |
| api/auth/trainer | Trainer | PUT | update data of currently logged in Trainer |

## Worker
| Endpoint Name | Who can | Method type | Purpose |
| ------- | ------------------------------- | ------- | ------------------------------- |
| api/auth/worker/all | Admin | GET | get the data of all Workers |
| api/auth/worker | Trainer | GET | get the data of currently logged in Worker |
| api/auth/worker/register | Guest | POST | register Worker account |
| api/auth/worker | Trainer | PUT | update data of currently logged in Worker |

# Carnets

## Allowed Entries Permission
| Endpoint Name | Who can | Method type | Purpose |
| ------- | ------------------------------- | ------- | ------------------------------- |
| api/carnets/allowed-entries-permission | Worker | GET | returns all AllowedEntriesPermissions created within the FitnessClub  |
| api/carnets/allowed-entries-permission/all-as-admin/{fitnessClubId} | Admin | GET | returns all AllowedEntriesPermissions created within the FitnessClub |
| api/carnets/allowed-entries-permission/{permissionId} | Worker, Admin | GET | returns a single AllowedEntriesPermission by id |
| api/carnets/allowed-entries-permission | Worker | POST | creates new AllowedEntriesPermission |
| api/carnets/allowed-entries-permission/{permissionId} | Worker | DELETE | deletes single AllowedEntriesPermission |

## Class Permission
| Endpoint Name | Who can | Method type | Purpose |
| ------- | ------------------------------- | ------- | ------------------------------- |
| api/carnets/class-permission | Worker | GET | returns all ClassPermissions created within the FitnessClub |
| api/carnets/class-permission/all-as-admin/{fitnessClubId} | Admin | GET | returns all ClassPermissions created within the FitnessClub |
| api/carnets/class-permission/{permissionId} | Worker, Admin | GET | returns a single ClassPermission by id |
| api/carnets/class-permission | Worker | POST | creates new ClassPermission |
| api/carnets/class-permission/{permissionId} | Worker | DELETE | deletes single ClassPermission |

## Gympass
| Endpoint Name | Who can | Method type | Purpose |
| ------- | ------------------------------- | ------- | ------------------------------- |
| api/carnets/gympass | Admin | GET | returns all Gympasses in the system |
| api/carnets/gympass/from-fitness-club | Worker | GET | returns all Gympasses from FitnessClub |
| api/carnets/gympass/{gympassId} | LoggedIn | GET | returns single Gympass by id |
| api/carnets/gympass | Member | POST | creates Gympass |
| api/carnets/gympass/cancel/{gympassId} | Member | PUT | cancells the Gympass |
| api/carnets/gympass/cancel-as-worker/{gympassId} | Worker | PUT | cancells the Gympass |
| api/carnets/gympass/activate/{gympassId} | Worker | PUT | activates the Gympass |
| api/carnets/gympass/deactivate/{gympassId} | Worker | PUT | deactivates the Gympass |

## Gympass Type
| Endpoint Name | Who can | Method type | Purpose |
| ------- | ------------------------------- | ------- | ------------------------------- |
| api/carnets/gympass-type/{gympasstypeId} | LoggedIn | GET | returns single GympassType by id |
| api/carnets/gympass-type/active-as-worker | Worker | GET | returns all active GympassTypes from FitnessClub |
| api/carnets/gympass-type/active/{fitnessClubId} | Member, Admin | GET | returns all active GympassTypes from FitnessClub |
| api/carnets/gympass-type | Worker | POST | creates GympassType |
| api/carnets/gympass-type/{gympasstypeId} | Worker | PUT | sets an existing GympassType inactive and returns a new, updated and active entity |
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

# Groups

# Notifications