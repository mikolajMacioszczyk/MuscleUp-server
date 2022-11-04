package groups.schedule.dto;

import java.util.UUID;

public record ScheduleGroupWorkoutDto(
        UUID groupWorkoutId,
        UUID workoutId,
        String location,
        int maxParticipants
) { }
