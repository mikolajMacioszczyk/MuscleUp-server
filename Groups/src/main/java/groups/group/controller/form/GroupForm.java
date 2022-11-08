package groups.group.controller.form;

import org.springframework.lang.Nullable;

import java.util.UUID;

public record GroupForm(
        String name,
        UUID trainerId,
        UUID fitnessClubId,
        @Nullable String description,
        String location,
        int maxParticipants,
        boolean repeatable
) { }
