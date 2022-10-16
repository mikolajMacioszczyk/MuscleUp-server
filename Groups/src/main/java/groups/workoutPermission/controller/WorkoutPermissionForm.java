package groups.workoutPermission.controller;

import java.util.UUID;

public record WorkoutPermissionForm(UUID groupWorkoutId, UUID permissionId) {
}
