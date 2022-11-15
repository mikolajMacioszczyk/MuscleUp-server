package content.workoutExerciseCriterionResult.repository;

import content.workoutExerciseCriterionResult.entity.WorkoutExerciseCriterionResult;

import java.util.UUID;

public interface WorkoutExerciseCriterionResultQuery {

    WorkoutExerciseCriterionResult getById(UUID id);

    int getDefaultValue(UUID userId, UUID workoutExerciseId, UUID criterionId);

    int getValue(UUID userId, UUID workoutExerciseId, UUID criterionId, UUID performedWorkoutId);
}
