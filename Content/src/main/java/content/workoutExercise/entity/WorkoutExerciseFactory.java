package content.workoutExercise.entity;

import content.exercise.repository.ExerciseRepository;
import content.workout.entity.Workout;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.util.Assert;

import java.util.UUID;

@Service
public class WorkoutExerciseFactory {

    private final ExerciseRepository exerciseRepository;


    @Autowired
    public WorkoutExerciseFactory(ExerciseRepository exerciseRepository) {

        Assert.notNull(exerciseRepository, "exerciseRepository must not be null");

        this.exerciseRepository = exerciseRepository;
    }

    public WorkoutExercise create(Workout workout, UUID exerciseId, int sequenceNumber) {

        return new WorkoutExercise(
                sequenceNumber,
                workout,
                exerciseRepository.getById(exerciseId)
        );
    }
}
