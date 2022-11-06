package groups.schedule.dto;

import java.time.LocalDateTime;
import java.util.UUID;

public record ScheduleGroupWorkoutDto(
        UUID groupWorkoutId,
        UUID workoutId,
        String location,
        int maxParticipants,
        LocalDateTime startTime,
        LocalDateTime endTime
) { }
