package groups.groupWorkout.entity;

import java.time.ZonedDateTime;
import java.util.UUID;

public record GroupWorkoutDto(
        UUID id,
        UUID groupId,
        UUID workoutId,
        ZonedDateTime startTime,
        ZonedDateTime endTime,
        UUID cloneId
) { }
