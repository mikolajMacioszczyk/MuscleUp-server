package groups.schedule.dto;

import java.util.UUID;

public record ScheduleGroupDto(
    UUID id,
    String name,
    String description,
    boolean repeatable
) { }
