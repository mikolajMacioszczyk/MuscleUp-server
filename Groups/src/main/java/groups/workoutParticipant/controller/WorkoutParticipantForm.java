package groups.workoutParticipant.controller;

import java.util.UUID;

public record WorkoutParticipantForm(UUID groupWorkoutId, UUID participantId) {

}
