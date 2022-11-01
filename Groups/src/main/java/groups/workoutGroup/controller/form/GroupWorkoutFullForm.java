package groups.workoutGroup.controller.form;

import java.time.LocalDateTime;
import java.util.UUID;

public record GroupWorkoutFullForm(UUID groupId, UUID workoutId, LocalDateTime startTime, LocalDateTime endTime) {
}
