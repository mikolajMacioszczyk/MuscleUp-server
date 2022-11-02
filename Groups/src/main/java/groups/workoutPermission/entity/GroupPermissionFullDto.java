package groups.workoutPermission.entity;

import java.util.UUID;

public record GroupPermissionFullDto(UUID id, UUID groupId, UUID permissionId) {
}
