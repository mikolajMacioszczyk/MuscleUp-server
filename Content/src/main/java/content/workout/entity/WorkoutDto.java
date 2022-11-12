package content.workout.entity;

import content.bodyPart.entity.BodyPartNameDto;
import content.exercise.entity.ExerciseNameAndImageDto;
import org.springframework.lang.Nullable;

import java.util.List;
import java.util.UUID;

public record WorkoutDto(
        UUID id,
        UUID creatorId,
        String description,
        @Nullable String videoUrl,
        List<ExerciseNameAndImageDto> exercises,
        List<BodyPartNameDto> bodyParts
) { }