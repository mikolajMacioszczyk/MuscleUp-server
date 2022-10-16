package groups.workoutParticipant.controller.form;

import java.util.UUID;

public record WorkoutParticipantForm(UUID groupWorkoutId, UUID participantId) {
}
