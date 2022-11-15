package content.workoutExerciseCriterionResult.repository;

import content.workoutExerciseCriterionResult.entity.WorkoutExerciseCriterionResult;

import java.util.UUID;

public interface WorkoutExerciseCriterionResultRepository {

    WorkoutExerciseCriterionResult getById(UUID id);

    WorkoutExerciseCriterionResult getByParams(UUID userId, UUID workoutExerciseId, UUID criterionId);

    UUID save(WorkoutExerciseCriterionResult workout);

    UUID merge(WorkoutExerciseCriterionResult workout);
}
