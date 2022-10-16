package groups.workoutPermission.controller.form;

import java.util.UUID;

public record WorkoutPermissionForm(UUID groupWorkoutId, UUID permissionId) {
}
