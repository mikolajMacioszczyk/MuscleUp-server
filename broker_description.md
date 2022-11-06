# Exchange: 'membershipExchange'

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

# Exchange: 'deletedPermissionExchange'

## Queue: 'deletedPermission'

### Routing Keys: 'deletedPermission'

Publishers:
- carnets  

Receivers:
- groups???  

Transmited Object:  
{  
    permissionId: string,
    permissionType: { 'classPermission', 'perkPermission' },
    fitnessClubId: string,
    permissionName: string
} 

Description:  
Inform about deleted permission

# Exchange: 'terminatedEmploymentExchange'

## Queue: 'terminatedEmployment'

### Routing Keys: 'terminatedEmployment'

Publishers:
- fitnessClubs  

Receivers:
- groups???  

Transmited Object:  
{  
    employmentId: string,
    userId: string
    fitnessClubId: string,
    employedFrom: DateTime (UTC),
    employedTo: DateTime? (UTC),
    isActive: boolean
} 

Description:  
Inform about terminated trainer employment