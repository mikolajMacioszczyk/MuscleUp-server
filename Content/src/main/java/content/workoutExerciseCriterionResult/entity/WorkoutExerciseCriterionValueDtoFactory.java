package content.workoutExerciseCriterionResult.entity;

import content.criterion.entity.Criterion;
import content.workoutExercise.entity.WorkoutExercise;
import content.workoutExerciseCriterionResult.repository.WorkoutExerciseCriterionResultQuery;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.util.Assert;

import java.util.UUID;

import static java.util.Objects.isNull;

@Service
public class WorkoutExerciseCriterionValueDtoFactory {

    private final WorkoutExerciseCriterionResultQuery workoutExerciseCriterionResultQuery;


    @Autowired
    public WorkoutExerciseCriterionValueDtoFactory(WorkoutExerciseCriterionResultQuery workoutExerciseCriterionResultQuery) {

        Assert.notNull(workoutExerciseCriterionResultQuery, "workoutExerciseCriterionResultQuery must not be null");

        this.workoutExerciseCriterionResultQuery = workoutExerciseCriterionResultQuery;
    }


    public WorkoutExerciseCriterionValueDto create(Criterion criterion, WorkoutExercise workoutExercise, UUID userId, UUID performedWorkoutId) {

        return new WorkoutExerciseCriterionValueDto(
                criterion.getId(),
                criterion.getName(),
                criterion.getUnit(),
                workoutExerciseCriterionResultQuery.getValue(
                        userId,
                        workoutExercise.getId(),
                        criterion.getId(),
                        performedWorkoutId
                )
        );
    }
}
