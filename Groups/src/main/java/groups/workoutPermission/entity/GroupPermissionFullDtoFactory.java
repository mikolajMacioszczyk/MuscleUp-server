package groups.workoutPermission.entity;

import org.springframework.util.Assert;

public class GroupPermissionFullDtoFactory {

    public GroupPermissionFullDto create(GroupPermission groupPermission) {

        Assert.notNull(groupPermission, "workoutPermission must not be null");

        return new GroupPermissionFullDto(
                groupPermission.getId(),
                groupPermission.getGroup().getId(),
                groupPermission.getPermissionId()
        );
    }
}
