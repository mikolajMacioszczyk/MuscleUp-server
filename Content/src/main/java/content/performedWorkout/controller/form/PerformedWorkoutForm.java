package content.performedWorkout.controller.form;

import org.springframework.lang.Nullable;

import java.time.ZonedDateTime;
import java.util.UUID;

public record PerformedWorkoutForm(
        UUID workoutId,
        UUID userId,
        ZonedDateTime time,
        @Nullable UUID entryId
) { }
