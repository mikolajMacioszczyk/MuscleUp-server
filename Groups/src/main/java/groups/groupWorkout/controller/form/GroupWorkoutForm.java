package groups.groupWorkout.controller.form;

import java.time.ZonedDateTime;
import java.util.UUID;

public record GroupWorkoutForm(
        UUID groupId,
        UUID workoutId,
        ZonedDateTime startTime,
        ZonedDateTime endTime
) { }
