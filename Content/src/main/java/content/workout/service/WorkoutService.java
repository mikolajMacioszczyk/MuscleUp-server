package content.workout.service;

import content.bodyPart.entity.BodyPart;
import content.bodyPart.repository.BodyPartRepository;
import content.performedWorkout.repository.PerformedWorkoutQuery;
import content.workout.controller.form.WorkoutForm;
import content.workout.entity.*;
import content.workout.repository.WorkoutQuery;
import content.workout.repository.WorkoutRepository;
import content.workoutExercise.entity.WorkoutExercise;
import content.workoutExercise.service.WorkoutExerciseService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.util.Assert;

import java.util.ArrayList;
import java.util.List;
import java.util.UUID;

import static java.util.Collections.sort;

@Service
public class WorkoutService {

    private final WorkoutRepository workoutRepository;
    private final WorkoutExerciseService workoutExerciseService;
    private final BodyPartRepository bodyPartRepository;
    private final WorkoutFactory workoutFactory;
    private final WorkoutQuery workoutQuery;
    private final PerformedWorkoutQuery performedWorkoutQuery;


    @Autowired
    public WorkoutService(WorkoutRepository workoutRepository,
                          WorkoutExerciseService workoutExerciseService,
                          BodyPartRepository bodyPartRepository,
                          WorkoutQuery workoutQuery,
                          PerformedWorkoutQuery performedWorkoutQuery) {

        Assert.notNull(workoutRepository, "workoutRepository must not be null");
        Assert.notNull(workoutExerciseService, "workoutExerciseService must not be null");
        Assert.notNull(bodyPartRepository, "bodyPartRepository must not be null");
        Assert.notNull(workoutQuery, "workoutQuery must not be null");
        Assert.notNull(performedWorkoutQuery, "performedWorkoutQuery must not be null");

        this.workoutRepository = workoutRepository;
        this.workoutExerciseService = workoutExerciseService;
        this.bodyPartRepository = bodyPartRepository;
        this.workoutQuery = workoutQuery;
        this.performedWorkoutQuery = performedWorkoutQuery;
        this.workoutFactory = new WorkoutFactory();
    }


    public UUID saveWorkout(WorkoutForm workoutForm) {

        Assert.notNull(workoutForm, "workoutForm must not be null");

        Workout workout = workoutFactory.createWithoutConnections(workoutForm);

        workout.setId(workoutRepository.save(workout));

        List<WorkoutExercise> workoutExercises = workoutExerciseService.collectiveSave(workout, workoutForm.exercises());
        List<BodyPart> bodyParts = bodyPartRepository.getByIds(workoutForm.bodyParts());

        workoutExercises.forEach(workout::addWorkoutExercise);
        bodyParts.forEach(workout::addBodyPart);

        return workoutRepository.update(workout);
    }

    public UUID updateWorkout(UUID id, WorkoutForm form) {

        WorkoutComparisonDto original = workoutQuery.getForComparison(id);

        if (form.isSimpleChange(original)) {

            Workout workout = workoutRepository.getById(id);

            workout.setDescription(form.description());
            workout.setVideoUrl(form.videoUrl());

            return workoutRepository.update(workout);
        }
        else if (form.hasChanged(original)) {

            return saveWorkout(form);
        }

        else return original.id();
    }

    public void deleteWorkout(UUID id) {

        Assert.notNull(id, "id must not be null");

        workoutRepository.delete(id);
    }

    public List<WorkoutPopularDto> getPopularWorkouts(int pieces) {

        List<WorkoutPopularDto> popularityRanking = new ArrayList<>();

        List<WorkoutDto> workouts = workoutQuery.getAllWorkouts();

        workouts.forEach(workout ->
                popularityRanking.add(
                        new WorkoutPopularDto(
                                workout.description(),
                                performedWorkoutQuery.getPerformancesByWorkoutId(workout.id())
                        )
                )
        );

        sort(popularityRanking);

        return pieces > popularityRanking.size()?
                popularityRanking :
                popularityRanking.subList(0, pieces-1);
    }
}
