package content.workout.entity;

import content.bodyPart.entity.BodyPartNameDtoFactory;
import org.springframework.util.Assert;

public class WorkoutDtoFactory {

    private final BodyPartNameDtoFactory bodyPartNameDtoFactory;


    public WorkoutDtoFactory() {

        this.bodyPartNameDtoFactory = new BodyPartNameDtoFactory();
    }


    public WorkoutDto create(Workout workout) {

        Assert.notNull(workout, "workout must not be null");

        return new WorkoutDto(
                workout.getId(),
                workout.getCreatorId(),
                workout.getDescription(),
                workout.getVideoUrl(),
                workout.getBodyParts()
                        .stream()
                        .map(bodyPartNameDtoFactory::create)
                        .toList()
        );
    }
}
