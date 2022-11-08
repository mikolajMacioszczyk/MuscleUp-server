package groups.schedule.dto;

import java.util.UUID;

public record ScheduleGroupDto(
    UUID id,
    String name,
    String description,
    String location,
    int maxParticipants,
    boolean repeatable,
    UUID fitnessClub
) { }
