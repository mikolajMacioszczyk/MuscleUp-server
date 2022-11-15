package content.workoutExerciseCriterionResult.entity;

import java.util.UUID;

public record WorkoutExerciseCriterionValueDto(UUID id, String name, String unit, int value) {
}
