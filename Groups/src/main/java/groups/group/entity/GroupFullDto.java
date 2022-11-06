package groups.group.entity;

import org.springframework.lang.Nullable;

import java.util.UUID;

public record GroupFullDto(
        UUID id,
        String name,
        @Nullable String description,
        boolean repeatable
) { }
