package content.workoutExercise.entity;

import content.workoutExerciseCriterionResult.entity.WorkoutExerciseCriterionValueDto;

import java.util.List;
import java.util.UUID;

public record WorkoutExerciseDto(
        UUID workoutExerciseId,
        UUID exerciseId,
        String name,
        String description,
        String imageUrl,
        List<WorkoutExerciseCriterionValueDto> criterionValues) {
}
