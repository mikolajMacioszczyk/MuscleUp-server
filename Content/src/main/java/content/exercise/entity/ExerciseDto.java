package content.exercise.entity;

import org.springframework.lang.Nullable;

import java.util.List;
import java.util.UUID;

public record ExerciseDto(
        UUID id,
        String name,
        String description,
        @Nullable String imageUrl,
        @Nullable String videoUrl,
        List<UUID> criterionIds
) { }