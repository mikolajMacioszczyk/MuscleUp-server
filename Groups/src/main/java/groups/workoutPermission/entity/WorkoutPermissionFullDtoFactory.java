package groups.workoutPermission.entity;

import org.springframework.util.Assert;

public class WorkoutPermissionFullDtoFactory {

    public WorkoutPermissionFullDto create(WorkoutPermission workoutPermission) {

        Assert.notNull(workoutPermission, "workoutPermission must not be null");

        return new WorkoutPermissionFullDto(
                workoutPermission.getId(),
                workoutPermission.getGroupWorkout().getId(),
                workoutPermission.getPermissionId()
        );
    }
}
