package groups.groupWorkout.entity;

import org.springframework.util.Assert;

public class GroupWorkoutDtoFactory {

    public GroupWorkoutDto create(GroupWorkout groupWorkout) {

        Assert.notNull(groupWorkout, "groupWorkout must not be null");

        return new GroupWorkoutDto(
                groupWorkout.getId(),
                groupWorkout.getGroup().getId(),
                groupWorkout.getWorkoutId(),
                groupWorkout.getStartTime(),
                groupWorkout.getEndTime(),
                groupWorkout.getCloneId()
        );
    }
}
