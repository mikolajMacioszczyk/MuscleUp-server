package groups.groupPermission.entity;

import java.util.UUID;

public record GroupPermissionFullDto(UUID id, UUID groupId, UUID permissionId) {
}
