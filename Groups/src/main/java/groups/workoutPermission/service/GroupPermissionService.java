package groups.workoutPermission.service;

import groups.workoutPermission.controller.form.GroupPermissionForm;
import groups.workoutPermission.entity.GroupPermission;
import groups.workoutPermission.entity.GroupPermissionFactory;
import groups.workoutPermission.repository.GroupPermissionRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.util.Assert;

import java.util.UUID;

@Service
public class GroupPermissionService {

    private final GroupPermissionRepository groupPermissionRepository;
    private final GroupPermissionFactory groupPermissionFactory;


    @Autowired
    private GroupPermissionService(GroupPermissionRepository groupPermissionRepository, GroupPermissionFactory groupPermissionFactory) {

        Assert.notNull(groupPermissionRepository, "groupPermissionRepository must not be null");
        Assert.notNull(groupPermissionFactory, "groupPermissionFactory must not be null");

        this.groupPermissionRepository = groupPermissionRepository;
        this.groupPermissionFactory = groupPermissionFactory;
    }


    public UUID add(GroupPermissionForm groupPermissionForm) {

        Assert.notNull(groupPermissionForm, "workoutPermissionForm must not be null");

        GroupPermission groupPermission = groupPermissionFactory.create(groupPermissionForm);

        return groupPermissionRepository.add(groupPermission);
    }

    public void remove(UUID workoutPermissionId) {

        Assert.notNull(workoutPermissionId, "workoutPermissionId must not be null");

        groupPermissionRepository.remove(workoutPermissionId);
    }

    public void remove(UUID groupWorkoutId, UUID permissionId) {

        Assert.notNull(groupWorkoutId, "groupWorkoutId must not be null");
        Assert.notNull(permissionId, "permissionId must not be null");

        groupPermissionRepository.remove(groupWorkoutId, permissionId);
    }

    public void unassignAllByGroupWorkoutId(UUID groupWorkoutId) {

        Assert.notNull(groupWorkoutId, "groupWorkoutId must not be null");

        groupPermissionRepository.unassignAllByGroupId(groupWorkoutId);
    }
}