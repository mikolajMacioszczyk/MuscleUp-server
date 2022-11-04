package groups.schedule.dto;

import groups.groupPermission.entity.GroupPermission;
import groups.groupWorkout.entity.GroupWorkout;
import groups.workoutParticipant.entity.WorkoutParticipant;
import org.springframework.util.Assert;

public class ScheduleCellHolderFactory {

    private final ScheduleGroupDtoFactory scheduleGroupDtoFactory;
    private final ScheduleGroupWorkoutDtoFactory scheduleGroupWorkoutDtoFactory;
    private final ScheduleTrainerDtoFactory scheduleTrainerDtoFactory;


    public ScheduleCellHolderFactory() {

        this.scheduleGroupDtoFactory = new ScheduleGroupDtoFactory();
        this.scheduleGroupWorkoutDtoFactory = new ScheduleGroupWorkoutDtoFactory();
        this.scheduleTrainerDtoFactory = new ScheduleTrainerDtoFactory();
    }


    public ScheduleCellHolder create(GroupWorkout groupWorkout) {

        Assert.notNull(groupWorkout, "groupWorkout must not be null");

        ScheduleCellHolder cellHolder = new ScheduleCellHolder();

        cellHolder.setGroup(scheduleGroupDtoFactory.create(groupWorkout.getGroup()));
        cellHolder.setWorkout(scheduleGroupWorkoutDtoFactory.create(groupWorkout));
        cellHolder.setTrainer(scheduleTrainerDtoFactory.createEmpty(groupWorkout.getGroup()));
        cellHolder.setParticipants(
                groupWorkout
                        .getWorkoutParticipants()
                        .stream()
                        .map(WorkoutParticipant::getGympassId)
                        .toList()
        );
        cellHolder.setPermissions(
                groupWorkout
                        .getGroup()
                        .getGroupPermissions()
                        .stream()
                        .map(GroupPermission::getPermissionId)
                        .toList()
        );

        return cellHolder;
    }
}
