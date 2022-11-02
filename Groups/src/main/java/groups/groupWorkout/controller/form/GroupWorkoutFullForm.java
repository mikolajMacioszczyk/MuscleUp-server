package groups.groupWorkout.controller.form;

import java.util.UUID;

public record GroupWorkoutFullForm(UUID groupId, UUID workoutId, String location, int maxParticipants) {
}
