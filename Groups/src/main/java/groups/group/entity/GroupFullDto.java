package groups.group.entity;

import java.time.LocalDateTime;
import java.util.UUID;

public record GroupFullDto(
        UUID id,
        String name,
        String description,
        LocalDateTime startTime,
        LocalDateTime endTime,
        Boolean repeatable
) { }
