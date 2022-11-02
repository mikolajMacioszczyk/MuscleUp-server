package groups.workoutPermission.controller.form;

import java.util.UUID;

public record GroupPermissionForm(UUID groupId, UUID permissionId) {
}
