package content.workoutExercise.entity;

import java.util.UUID;

public record WorkoutExerciseForm(
        UUID workoutId,
        UUID exerciseId,
        int sequenceNumber
) { }
