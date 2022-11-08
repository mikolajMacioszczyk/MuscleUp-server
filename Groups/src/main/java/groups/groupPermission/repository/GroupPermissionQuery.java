package groups.groupPermission.repository;

import groups.groupPermission.entity.GroupPermission;
import groups.groupPermission.entity.GroupPermissionDto;

import java.util.List;
import java.util.UUID;

public interface GroupPermissionQuery {

    GroupPermission getById(UUID id);

    List<GroupPermissionDto> getAllGroupPermissionsByGroupIdAndPermissionId(UUID groupId, UUID permissionId);
}
