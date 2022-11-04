package groups.schedule.dto;

import java.util.UUID;

public record ScheduleTrainerDto(
        UUID trainerId,
        String name,
        String surname,
        String avatarUrl
) { }
