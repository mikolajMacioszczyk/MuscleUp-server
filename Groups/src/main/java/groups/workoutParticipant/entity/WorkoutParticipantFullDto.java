package groups.workoutParticipant.entity;

import java.util.UUID;

public record WorkoutParticipantFullDto(UUID id, UUID groupWorkoutId, UUID participantId) {

}
