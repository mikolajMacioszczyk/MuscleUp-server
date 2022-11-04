package groups.schedule.dto;

import java.time.LocalDateTime;
import java.util.UUID;

public record ScheduleGroupDto(
    UUID id,
    String name,
    String description,
    LocalDateTime startTime,
    LocalDateTime endTime,
    boolean repeatable
) { }
