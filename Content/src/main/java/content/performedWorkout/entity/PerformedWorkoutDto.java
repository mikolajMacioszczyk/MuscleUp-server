package content.performedWorkout.entity;

import org.springframework.lang.Nullable;

import java.time.ZonedDateTime;
import java.util.UUID;

public record PerformedWorkoutDto(
        UUID id,
        String workoutName,
        UUID userId,
        ZonedDateTime time,
        @Nullable UUID entryId
) { }
