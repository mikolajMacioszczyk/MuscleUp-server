package groups.workoutPermission.repository;

import groups.workoutPermission.entity.WorkoutPermission;
import groups.workoutPermission.entity.WorkoutPermissionFullDto;

import java.util.List;
import java.util.Optional;
import java.util.UUID;

public interface WorkoutPermissionQuery {

    WorkoutPermission getById(UUID id);

    List<WorkoutPermissionFullDto> getAllWorkoutPermissions();

    Optional<WorkoutPermissionFullDto> findWorkoutPermissionById(UUID id);

    List<WorkoutPermissionFullDto> getAllWorkoutPermissionsByGroupWorkoutId(UUID groupWorkoutId);

    List<WorkoutPermissionFullDto> getAllWorkoutPermissionsByPermissionId(UUID participantId);

    List<WorkoutPermissionFullDto> getAllWorkoutPermissionsByGroupWorkoutIdAndPermissionId(UUID groupWorkoutId, UUID permissionId);
}
