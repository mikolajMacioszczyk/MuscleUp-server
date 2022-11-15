package content.performedWorkout.controller.form;

import content.workout.controller.form.ExerciseValueForm;

import java.time.ZonedDateTime;
import java.util.List;
import java.util.UUID;

public record PerformedWorkoutForm(
        UUID workoutId,
        UUID userId,
        ZonedDateTime time,
        List<ExerciseValueForm> exercises
) { }
