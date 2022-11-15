package content.workout.entity;

import content.bodyPart.entity.BodyPart;
import content.workoutExercise.entity.WorkoutExercise;
import org.springframework.util.Assert;

public class WorkoutComparisonDtoFactory {

    public WorkoutComparisonDto create(Workout workout) {

        Assert.notNull(workout, "workout must not be null");

        return new WorkoutComparisonDto(
                workout.getId(),
                workout.getCreatorId(),
                workout.getDescription(),
                workout.getName(),
                workout.getBodyParts()
                        .stream()
                        .map(BodyPart::getId)
                        .toList(),
                workout.getWorkoutExercises()
                        .stream()
                        .map(WorkoutExercise::getId)
                        .toList()
        );
    }
}
