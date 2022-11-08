package groups.groupPermission.repository;

import groups.groupPermission.entity.GroupPermission;

import java.util.UUID;

public interface GroupPermissionRepository {

    GroupPermission getById(UUID id);

    UUID add(GroupPermission groupPermission);

    void remove(UUID groupId, UUID permissionId);
}
