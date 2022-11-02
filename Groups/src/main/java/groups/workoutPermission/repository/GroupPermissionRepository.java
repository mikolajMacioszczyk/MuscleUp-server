package groups.workoutPermission.repository;

import groups.workoutPermission.entity.GroupPermission;

import java.util.UUID;

public interface GroupPermissionRepository {

    GroupPermission getById(UUID id);

    UUID add(GroupPermission groupPermission);

    void remove(UUID groupPermissionId);

    void remove(UUID groupId, UUID permissionId);

    void unassignAllByGroupId(UUID groupId);
}
