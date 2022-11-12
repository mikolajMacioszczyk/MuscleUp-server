package content.workout.entity;

import content.workout.controller.form.WorkoutForm;
import org.springframework.util.Assert;

import java.util.ArrayList;

public class WorkoutFactory {

    public Workout createWithoutConnections(WorkoutForm workoutForm) {

        Assert.notNull(workoutForm, "workoutForm must not be null");

        return new Workout(
                workoutForm.creatorId(),
                workoutForm.description(),
                workoutForm.videoUrl(),
                new ArrayList<>(),
                new ArrayList<>()
        );
    }
}
