package content.workoutExercise.service;

import content.workout.controller.form.ExerciseValueForm;
import content.workout.entity.Workout;
import content.workoutExercise.entity.WorkoutExercise;
import content.workoutExercise.entity.WorkoutExerciseFactory;
import content.workoutExercise.repository.WorkoutExerciseRepository;
import content.workoutExerciseCriterionResult.service.WorkoutExerciseCriterionResultService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.util.Assert;

import java.util.ArrayList;
import java.util.List;
import java.util.UUID;

@Service
public class WorkoutExerciseService {

    private final WorkoutExerciseRepository workoutExerciseRepository;
    private final WorkoutExerciseFactory workoutExerciseFactory;
    private final WorkoutExerciseCriterionResultService workoutExerciseCriterionResultService;


    @Autowired
    public WorkoutExerciseService(WorkoutExerciseRepository workoutExerciseRepository,
                                  WorkoutExerciseFactory workoutExerciseFactory,
                                  WorkoutExerciseCriterionResultService workoutExerciseCriterionResultService) {

        Assert.notNull(workoutExerciseRepository, "workoutExerciseRepository must not be null");
        Assert.notNull(workoutExerciseFactory, "workoutExerciseFactory must not be null");
        Assert.notNull(workoutExerciseCriterionResultService, "workoutExerciseCriterionResultService must not be null");

        this.workoutExerciseRepository = workoutExerciseRepository;
        this.workoutExerciseFactory = workoutExerciseFactory;
        this.workoutExerciseCriterionResultService = workoutExerciseCriterionResultService;
    }


    public List<WorkoutExercise> collectiveSave(Workout workout, List<ExerciseValueForm> exercises, UUID userId) {

        List<WorkoutExercise> workoutExercises = new ArrayList<>();

        for(int i=0; i<exercises.size(); i++) {

            WorkoutExercise workoutExercise = workoutExerciseFactory.create(workout, exercises.get(i).exerciseId(), i);
            workoutExerciseRepository.save(workoutExercise);

            exercises.get(i).criterionValues().forEach(
                    criterionValueForm ->
                        workoutExerciseCriterionResultService.saveDefaultResult(
                                userId,
                                workoutExercise,
                                criterionValueForm.criterionId(),
                                criterionValueForm.value())
            );

            workoutExercises.add(workoutExercise);
        }

        return workoutExercises;
    }
}