# Exchange: 'muscleUpExchange'

## Queue: 'memberships'

### Routing Keys: 'memberships'

Publishers:
- carnets  

Receivers:
- fitnessClubs  

Transmited Object:  
{  
    memberId: string,  
    fitnessClubId: string  
}  

Description:  
Creates a Member Membership in the FitnessClub, if not yet created.