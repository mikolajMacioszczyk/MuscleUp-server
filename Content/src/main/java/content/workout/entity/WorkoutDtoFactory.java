package content.workout.entity;

import content.bodyPart.entity.BodyPartNameDtoFactory;
import content.common.user.UserQuery;
import content.workoutExercise.entity.WorkoutExerciseDtoFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.util.Assert;

import java.util.UUID;

@Service
public class WorkoutDtoFactory {

    private final BodyPartNameDtoFactory bodyPartNameDtoFactory;
    private final WorkoutExerciseDtoFactory workoutExerciseDtoFactory;
    private final UserQuery userQuery;


    @Autowired
    public WorkoutDtoFactory(UserQuery userQuery, WorkoutExerciseDtoFactory workoutExerciseDtoFactory) {

        Assert.notNull(userQuery, "userQuery must not be null");
        Assert.notNull(workoutExerciseDtoFactory, "workoutExerciseDtoFactory must not be null");

        this.userQuery = userQuery;
        this.workoutExerciseDtoFactory = workoutExerciseDtoFactory;
        this.bodyPartNameDtoFactory = new BodyPartNameDtoFactory();
    }


    public WorkoutDto create(Workout workout, UUID performedWorkoutId) {

        Assert.notNull(workout, "workout must not be null");

        return new WorkoutDto(
                workout.getId(),
                userQuery.getUserById(workout.getCreatorId()),
                workout.getDescription(),
                workout.getName(),
                workout.getWorkoutExercises()
                        .stream()
                        .map(workoutExercise -> workoutExerciseDtoFactory.create(workoutExercise, workout.getCreatorId(), performedWorkoutId))
                        .toList(),
                workout.getBodyParts()
                        .stream()
                        .map(bodyPartNameDtoFactory::create)
                        .toList()
        );
    }
}
