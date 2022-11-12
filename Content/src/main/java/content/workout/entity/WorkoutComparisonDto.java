package content.workout.entity;

import org.springframework.lang.Nullable;

import java.util.List;
import java.util.UUID;

// TODO replace all comparison dtos to form with factory
public record WorkoutComparisonDto(
        UUID id,
        UUID creatorId,
        String description,
        @Nullable String videoUrl,
        List<UUID> bodyParts,
        List<UUID> exercises
) { }
