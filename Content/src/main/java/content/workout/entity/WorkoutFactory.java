package content.workout.entity;

import content.workout.controller.form.WorkoutForm;
import org.springframework.util.Assert;

import java.util.ArrayList;
import java.util.UUID;

public class WorkoutFactory {

    public Workout createWithoutConnections(UUID fitnessClubId, WorkoutForm workoutForm) {

        Assert.notNull(workoutForm, "workoutForm must not be null");

        return new Workout(
                workoutForm.creatorId(),
                fitnessClubId,
                workoutForm.description(),
                workoutForm.name(),
                true,
                new ArrayList<>(),
                new ArrayList<>()
        );
    }
}
