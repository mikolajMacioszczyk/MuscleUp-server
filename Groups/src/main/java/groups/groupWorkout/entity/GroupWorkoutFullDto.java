package groups.groupWorkout.entity;

import java.util.UUID;

public record GroupWorkoutFullDto(UUID id, UUID groupId, UUID workoutId, String location, int maxParticipants) {
}
