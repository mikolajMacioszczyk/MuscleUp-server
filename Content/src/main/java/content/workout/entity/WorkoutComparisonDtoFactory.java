package content.workout.entity;

import content.bodyPart.entity.BodyPart;
import content.exercise.entity.Exercise;
import content.workoutExercise.entity.WorkoutExercise;
import org.springframework.util.Assert;

public class WorkoutComparisonDtoFactory {

    public WorkoutComparisonDto create(Workout workout) {

        Assert.notNull(workout, "workout must not be null");

        return new WorkoutComparisonDto(
                workout.getId(),
                workout.getCreatorId(),
                workout.getDescription(),
                workout.getVideoUrl(),
                workout.getWorkoutExercises()
                        .stream()
                        .map(WorkoutExercise::getExercise)
                        .map(Exercise::getId)
                        .toList(),
                workout.getBodyParts()
                        .stream()
                        .map(BodyPart::getId)
                        .toList()
        );
    }
}
