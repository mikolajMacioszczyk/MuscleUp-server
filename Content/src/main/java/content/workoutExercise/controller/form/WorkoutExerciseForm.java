package content.workoutExercise.controller.form;

import java.util.UUID;

public record WorkoutExerciseForm(
        UUID workoutId,
        UUID exerciseId,
        int sequenceNumber
) { }
