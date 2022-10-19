# Notes
The default data is consistent. This means, for example, that the worker is already employed in the fitness club where the gympass data is set up.  
Available only when environement is "Development".  

# Auth

### Worker
- Email: worker@gmail.com  
- Password: 123Pa$$word.  
- Id: 747f6c50-aabc-4be2-aa43-10cf60aba46b  

### Trainer
- Email: trainer@gmail.com  
- Password: 123Pa$$word.  
- Id: c7c25024-1997-4c9b-9e37-fe150d0faac1  

### Member
- Email: member@gmail.com  
- Password: 123Pa$$word.  
- Id: faf314bc-1175-46b0-a57c-d456f09ca139  

# Fitness Clubs

### Fitness Club
- Id: 42b2ab44-d451-4f9d-9e98-c1734e6006f6  

### Worker Employment
- Id: 6fae2655-a372-4fe1-9949-5574d78dc85f  

# Carnets

### Gympass Type
- Id: 0184fafb-f3f9-47bc-8aae-e3b458a0670d

### ClassPermission
- Id: 0e737124-da70-40d5-b3fb-37f474050240

### Gympass
- Id: b967911e-7c9c-4f8e-b55e-8a5b46b8abfb

### Subscription
- Id: 9e2e85f7-5161-4dba-8674-96ba03de7098


# Groups

### Group
- Id: '3fa85f64-5717-4562-b3fc-2c963f66afa6'
- Name: 'Yoga'
- Max participants: 15

### GroupWorkout
- Id: '163cc334-4f81-11ed-bdc3-0242ac120002'
- workoutId: '37517ba0-4f81-11ed-bdc3-0242ac120002'
- classId: '3fa85f64-5717-4562-b3fc-2c963f66afa6'
- startDate: '2022-10-19T07:00:00+02:00'
- endDate: '2022-10-19T08:00:00+02:00'

### GroupTrainer
- Id: '8d553154-4f81-11ed-bdc3-0242ac120002'
- trainerId: 'c7c25024-1997-4c9b-9e37-fe150d0faac1'
- groupId: '3fa85f64-5717-4562-b3fc-2c963f66afa6'

### GroupWorkoutParticipant
- Id: 'df5943c8-4f81-11ed-bdc3-0242ac120002'
- gympassId: 'b967911e-7c9c-4f8e-b55e-8a5b46b8abfb'
- groupWorkoutId: '163cc334-4f81-11ed-bdc3-0242ac120002'

### GroupWorkoutPermission
- Id: '69096c1a-4f82-11ed-bdc3-0242ac120002'
- permissionId: '0e737124-da70-40d5-b3fb-37f474050240'
- groupWorkoutId: '163cc334-4f81-11ed-bdc3-0242ac120002'
- 