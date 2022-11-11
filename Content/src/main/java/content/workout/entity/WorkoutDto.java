package content.workout.entity;

import content.bodyPart.entity.BodyPartNameDto;
import org.springframework.lang.Nullable;

import java.util.List;
import java.util.UUID;

public record WorkoutDto(
        UUID id,
        UUID creatorId,
        String description,
        @Nullable String videoUrl,
        List<BodyPartNameDto> bodyParts
) { }