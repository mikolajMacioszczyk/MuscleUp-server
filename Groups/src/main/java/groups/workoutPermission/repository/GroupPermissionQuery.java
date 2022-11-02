package groups.workoutPermission.repository;

import groups.workoutPermission.entity.GroupPermission;
import groups.workoutPermission.entity.GroupPermissionFullDto;

import java.util.List;
import java.util.Optional;
import java.util.UUID;

public interface GroupPermissionQuery {

    GroupPermission getById(UUID id);

    List<GroupPermissionFullDto> getAllGroupPermissions();

    Optional<GroupPermissionFullDto> findGroupPermissionById(UUID id);

    List<GroupPermissionFullDto> getAllGroupPermissionsByGroupId(UUID groupId);

    List<GroupPermissionFullDto> getAllGroupPermissionsByPermissionId(UUID permissionId);

    List<GroupPermissionFullDto> getAllGroupPermissionsByGroupIdAndPermissionId(UUID groupId, UUID permissionId);
}
