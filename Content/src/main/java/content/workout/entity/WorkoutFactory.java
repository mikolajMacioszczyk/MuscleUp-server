package content.workout.entity;

import content.workout.controller.form.WorkoutForm;
import org.springframework.util.Assert;

public class WorkoutFactory {

    public Workout create(WorkoutForm workoutForm, boolean latest) {

        Assert.notNull(workoutForm, "workoutForm must not be null");

        return new Workout(
                workoutForm.creatorId(),
                workoutForm.description(),
                workoutForm.videoUrl()
        );
    }
}
