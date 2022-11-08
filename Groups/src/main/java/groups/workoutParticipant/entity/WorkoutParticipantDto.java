package groups.workoutParticipant.entity;

import java.util.UUID;

public record WorkoutParticipantDto(UUID groupWorkoutId, UUID userId) {
}
