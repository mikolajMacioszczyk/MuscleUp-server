package groups.workoutPermission.entity;

import java.util.UUID;

public record WorkoutPermissionFullDto(UUID id, UUID groupWorkoutId, UUID permissionId) {
}
