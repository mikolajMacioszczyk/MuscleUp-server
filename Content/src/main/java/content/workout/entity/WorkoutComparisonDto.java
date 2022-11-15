package content.workout.entity;

import java.util.List;
import java.util.UUID;

// TODO replace all comparison dtos to form with factory
public record WorkoutComparisonDto(
        UUID id,
        UUID creatorId,
        String description,
        String name,
        List<UUID> bodyParts,
        List<UUID> workoutExercises
) { }
