package content.performedWorkout.entity;

import content.workout.entity.WorkoutDtoFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.util.Assert;

@Service
public class PerformedWorkoutDtoFactory {

    private final WorkoutDtoFactory workoutDtoFactory;


    @Autowired
    public PerformedWorkoutDtoFactory(WorkoutDtoFactory workoutDtoFactory) {

        Assert.notNull(workoutDtoFactory, "workoutDtoFactory must not be null");

        this.workoutDtoFactory = workoutDtoFactory;
    }


    public PerformedWorkoutDto create(PerformedWorkout performedWorkout) {

        return new PerformedWorkoutDto(
                workoutDtoFactory.create(performedWorkout.getWorkout(), performedWorkout.getId()),
                performedWorkout.getUserId(),
                performedWorkout.getTime()
        );
    }
}
