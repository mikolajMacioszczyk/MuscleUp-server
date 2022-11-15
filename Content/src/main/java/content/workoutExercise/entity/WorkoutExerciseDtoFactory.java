package content.workoutExercise.entity;

import content.workoutExerciseCriterionResult.entity.WorkoutExerciseCriterionValueDtoFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.util.Assert;

import java.util.UUID;

@Service
public class WorkoutExerciseDtoFactory {

    private final WorkoutExerciseCriterionValueDtoFactory workoutExerciseCriterionValueDtoFactory;


    @Autowired
    public WorkoutExerciseDtoFactory(WorkoutExerciseCriterionValueDtoFactory workoutExerciseCriterionValueDtoFactory) {

        Assert.notNull(workoutExerciseCriterionValueDtoFactory, "workoutExerciseCriterionValueDtoFactory must not be null");

        this.workoutExerciseCriterionValueDtoFactory = workoutExerciseCriterionValueDtoFactory;
    }


    public WorkoutExerciseDto create(WorkoutExercise workoutExercise, UUID userId, UUID performedWorkoutId) {

        return new WorkoutExerciseDto(
                workoutExercise.getId(),
                workoutExercise.getExercise().getId(),
                workoutExercise.getExercise().getName(),
                workoutExercise.getExercise().getDescription(),
                workoutExercise.getExercise().getImageUrl(),
                workoutExercise.getExercise()
                        .getCriteria()
                        .stream()
                        .map(criterion -> workoutExerciseCriterionValueDtoFactory.create(criterion, workoutExercise, userId, performedWorkoutId))
                        .toList()
        );
    }
}