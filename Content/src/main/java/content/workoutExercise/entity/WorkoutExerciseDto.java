package content.workoutExercise.entity;

import java.util.UUID;

public record WorkoutExerciseDto(
        UUID workoutId,
        UUID exerciseId,
        int sequenceNumber
) { }
