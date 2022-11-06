package groups.schedule.dto;

import groups.groupWorkout.entity.GroupWorkout;
import org.springframework.util.Assert;

public class ScheduleGroupWorkoutDtoFactory {

    public ScheduleGroupWorkoutDto create(GroupWorkout groupWorkout) {

        Assert.notNull(groupWorkout, "groupWorkout must not be null");

        return new ScheduleGroupWorkoutDto(
                groupWorkout.getId(),
                groupWorkout.getWorkoutId(),
                groupWorkout.getLocation(),
                groupWorkout.getMaxParticipants(),
                groupWorkout.getStartTime(),
                groupWorkout.getEndTime()
        );
    }
}
