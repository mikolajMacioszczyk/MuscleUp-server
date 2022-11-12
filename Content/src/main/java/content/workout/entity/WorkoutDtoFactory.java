package content.workout.entity;

import content.bodyPart.entity.BodyPartNameDtoFactory;
import content.exercise.entity.ExerciseNameAndImageDtoFactory;
import content.workoutExercise.entity.WorkoutExercise;
import org.springframework.util.Assert;

public class WorkoutDtoFactory {

    private final BodyPartNameDtoFactory bodyPartNameDtoFactory;
    private final ExerciseNameAndImageDtoFactory exerciseNameAndImageDtoFactory;


    public WorkoutDtoFactory() {

        this.bodyPartNameDtoFactory = new BodyPartNameDtoFactory();
        this.exerciseNameAndImageDtoFactory = new ExerciseNameAndImageDtoFactory();
    }


    public WorkoutDto create(Workout workout) {

        Assert.notNull(workout, "workout must not be null");

        return new WorkoutDto(
                workout.getId(),
                workout.getCreatorId(),
                workout.getDescription(),
                workout.getVideoUrl(),
                workout.getWorkoutExercises()
                        .stream()
                        .map(WorkoutExercise::getExercise)
                        .map(exerciseNameAndImageDtoFactory::create)
                        .toList(),
                workout.getBodyParts()
                        .stream()
                        .map(bodyPartNameDtoFactory::create)
                        .toList()
        );
    }
}
