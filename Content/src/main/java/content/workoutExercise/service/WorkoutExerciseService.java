package content.workoutExercise.service;

import content.workout.entity.Workout;
import content.workoutExercise.entity.WorkoutExercise;
import content.workoutExercise.entity.WorkoutExerciseFactory;
import content.workoutExercise.repository.WorkoutExerciseRepository;
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


    @Autowired
    public WorkoutExerciseService(WorkoutExerciseRepository workoutExerciseRepository,
                                  WorkoutExerciseFactory workoutExerciseFactory) {

        Assert.notNull(workoutExerciseRepository, "workoutExerciseRepository must not be null");
        Assert.notNull(workoutExerciseFactory, "workoutExerciseFactory must not be null");

        this.workoutExerciseRepository = workoutExerciseRepository;
        this.workoutExerciseFactory = workoutExerciseFactory;
    }



    public List<WorkoutExercise> collectiveSave(Workout workout, List<UUID> exerciseIds) {

        List<WorkoutExercise> workoutExercises = new ArrayList<>();

        for(int i=0; i<exerciseIds.size(); i++) {

            WorkoutExercise workoutExercise = workoutExerciseFactory.create(workout, exerciseIds.get(i), i);
            workoutExerciseRepository.save(workoutExercise);

            workoutExercises.add(workoutExercise);
        }

        return workoutExercises;
    }
}