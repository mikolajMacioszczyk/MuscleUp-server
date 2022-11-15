package content.workout.controller.form;

import org.springframework.lang.Nullable;

import java.util.List;
import java.util.UUID;

public record ExerciseValueForm(
        UUID exerciseId,
        @Nullable UUID workoutExerciseId,
        List<CriterionValueForm> criterionValues
) { }
