package groups.schedule.dto;

import java.time.ZonedDateTime;
import java.util.UUID;

public record ScheduleGroupWorkoutDto(
        UUID groupWorkoutId,
        UUID workoutId,
        ZonedDateTime startTime,
        ZonedDateTime endTime
) { }
