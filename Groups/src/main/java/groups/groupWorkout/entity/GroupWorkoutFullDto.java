package groups.groupWorkout.entity;

import java.time.LocalDateTime;
import java.util.UUID;

public record GroupWorkoutFullDto(
        UUID id,
        UUID groupId,
        UUID workoutId,
        String location,
        int maxParticipants,
        LocalDateTime startTime,
        LocalDateTime endTime
) { }
