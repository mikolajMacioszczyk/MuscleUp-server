package groups.workoutGroup.entity;

import java.time.LocalDateTime;
import java.util.UUID;

public record GroupWorkoutFullDto(UUID id, UUID groupId, UUID workoutId, LocalDateTime startTime, LocalDateTime endTime) {
}
