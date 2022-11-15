package content.performedWorkout.service;

import content.performedWorkout.controller.form.PerformedWorkoutForm;
import content.performedWorkout.entity.PerformedWorkout;
import content.performedWorkout.entity.PerformedWorkoutFactory;
import content.performedWorkout.repository.PerformedWorkoutRepository;
import content.workout.entity.Workout;
import content.workout.repository.WorkoutRepository;
import content.workoutExerciseCriterionResult.service.WorkoutExerciseCriterionResultService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.util.Assert;

import java.util.UUID;

@Service
public class PerformedWorkoutService {

    private final PerformedWorkoutRepository performedWorkoutRepository;
    private final PerformedWorkoutFactory performedWorkoutFactory;
    private final WorkoutRepository workoutRepository;
    private final WorkoutExerciseCriterionResultService workoutExerciseCriterionResultService;


    @Autowired
    public PerformedWorkoutService(PerformedWorkoutRepository performedWorkoutRepository,
                                   PerformedWorkoutFactory performedWorkoutFactory,
                                   WorkoutRepository workoutRepository,
                                   WorkoutExerciseCriterionResultService workoutExerciseCriterionResultService) {

        Assert.notNull(performedWorkoutRepository, "performedWorkoutRepository must not be null");
        Assert.notNull(workoutRepository, "workoutRepository must not be null");
        Assert.notNull(workoutExerciseCriterionResultService, "workoutExerciseCriterionResultService must not be null");
        Assert.notNull(performedWorkoutFactory, "performedWorkoutFactory must not be null");

        this.performedWorkoutRepository = performedWorkoutRepository;
        this.workoutRepository = workoutRepository;
        this.workoutExerciseCriterionResultService = workoutExerciseCriterionResultService;
        this.performedWorkoutFactory = performedWorkoutFactory;
    }


    public UUID savePerformedWorkout(PerformedWorkoutForm form) {

        Assert.notNull(form, "form must not be null");

        PerformedWorkout performedWorkout = performedWorkoutFactory.create(form);
        UUID performedWorkoutId = performedWorkoutRepository.save(performedWorkout);
        Workout workout = workoutRepository.getById(form.workoutId());

        workout.getWorkoutExercises().forEach(workoutExercise ->

            workoutExercise.getExercise().getCriteria().forEach(criterion ->

                workoutExerciseCriterionResultService.saveResult(

                        form.userId(),
                        workoutExercise,
                        criterion.getId(),
                        form.exercises()
                                .stream()
                                .filter(exercise -> exercise.exerciseId().equals(workoutExercise.getExercise().getId()))
                                .toList()
                                .get(0)
                                .criterionValues()
                                .stream()
                                .filter(crit -> crit.criterionId().equals(criterion.getId()))
                                .toList()
                                .get(0)
                                .value(),
                        performedWorkoutId
                        )
            )
        );

        return performedWorkoutId;
    }
}
