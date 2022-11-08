package groups.group.entity;

import org.springframework.lang.Nullable;

import java.util.UUID;

public record GroupDto(
        UUID id,
        String name,
        UUID trainerId,
        UUID fitnessClubId,
        @Nullable String description,
        String location,
        int maxParticipants,
        boolean repeatable
) { }
