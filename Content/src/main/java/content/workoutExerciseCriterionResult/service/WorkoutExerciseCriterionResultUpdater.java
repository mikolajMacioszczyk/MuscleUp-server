package content.workoutExerciseCriterionResult.service;

import content.workout.controller.form.WorkoutForm;
import content.workout.entity.Workout;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.util.Assert;

@Service
public class WorkoutExerciseCriterionResultUpdater {

    private final WorkoutExerciseCriterionResultService workoutExerciseCriterionResultService;


    @Autowired
    public WorkoutExerciseCriterionResultUpdater(WorkoutExerciseCriterionResultService workoutExerciseCriterionResultService) {

        Assert.notNull(workoutExerciseCriterionResultService, "workoutExerciseCriterionResultService must not be null");

        this.workoutExerciseCriterionResultService = workoutExerciseCriterionResultService;
    }


    public void updateAll(Workout workout, WorkoutForm form) {

        workout.getWorkoutExercises().forEach(workoutExercise ->

            workoutExercise.getExercise().getCriteria().forEach(criterion ->

                workoutExerciseCriterionResultService.updateDefaultResult(
                        workout.getCreatorId(),
                        workoutExercise.getId(),
                        criterion.getId(),
                        form.exercises()
                                .stream()
                                .filter(exercise -> exercise.workoutExerciseId().equals(workoutExercise.getId()))
                                .toList()
                                .get(0)
                                .criterionValues()
                                .stream()
                                .filter(crit -> crit.criterionId().equals(criterion.getId()))
                                .toList()
                                .get(0)
                                .value()
                )
            )
        );
    }
}
