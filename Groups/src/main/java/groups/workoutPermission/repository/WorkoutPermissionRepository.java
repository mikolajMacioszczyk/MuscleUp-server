package groups.workoutPermission.repository;

import groups.workoutPermission.entity.WorkoutPermission;

import java.util.UUID;

public interface WorkoutPermissionRepository {

    WorkoutPermission getById(UUID id);

    UUID add(WorkoutPermission workoutPermission);

    void remove(UUID workoutPermissionId);

    void remove(UUID groupWorkoutId, UUID permissionId);
}
