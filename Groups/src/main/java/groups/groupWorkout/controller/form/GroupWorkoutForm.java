package groups.groupWorkout.controller.form;

import java.time.LocalDateTime;
import java.util.UUID;

public record GroupWorkoutForm(
        UUID groupId,
        UUID workoutId,
        String location,
        int maxParticipants,
        LocalDateTime startTime,
        LocalDateTime endTime
) { }
