package groups.groupWorkout.entity;

import org.springframework.util.Assert;

public class GroupWorkoutFullDtoFactory {

    public GroupWorkoutFullDto create(GroupWorkout groupWorkout) {

        Assert.notNull(groupWorkout, "groupWorkout must not be null");

        return new GroupWorkoutFullDto(
                groupWorkout.getId(),
                groupWorkout.getGroup().getId(),
                groupWorkout.getWorkoutId(),
                groupWorkout.getLocation(),
                groupWorkout.getMaxParticipants(),
                groupWorkout.getStartTime(),
                groupWorkout.getEndTime()
        );
    }
}
