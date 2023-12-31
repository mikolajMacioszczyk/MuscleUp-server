package groups.schedule.dto;

import org.springframework.util.Assert;

import java.util.List;
import java.util.UUID;

import static groups.common.utils.StringUtils.isNullOrEmpty;
import static groups.group.controller.GroupValidator.MIN_PARTICIPANTS;
import static java.util.Objects.isNull;

public class ScheduleCellHolder {

    private final ScheduleCell scheduleCell;


    public ScheduleCellHolder() {

        this.scheduleCell = new ScheduleCell();
    }


    public ScheduleCell getValidScheduleCell() {

        Assert.isTrue(isValid(), "scheduleCell must be valid");

        return scheduleCell;
    }

    public void setGroup(ScheduleGroupDto group) {
        this.scheduleCell.setGroup(group);
    }

    public void setWorkout(ScheduleGroupWorkoutDto workout) {
        this.scheduleCell.setWorkout(workout);
    }

    public void setTrainer(ScheduleTrainerDto trainer) {
        this.scheduleCell.setTrainer(trainer);
    }

    public void setParticipants(List<UUID> participants) {
        this.scheduleCell.setParticipants(participants);
    }

    public void setPermissions(List<UUID> permissions) {
        this.scheduleCell.setPermissions(permissions);
    }

    public UUID getTrainerId() {
        return scheduleCell.getTrainer().trainerId();
    }

    public boolean isValid() {

        return validGroup()
                && validWorkout()
                && validTrainer()
                && validParticipants()
                && validPermissions();
    }


    private boolean validGroup() {

        return !isNull(scheduleCell.getGroup())
                && !isNull(scheduleCell.getGroup().id())
                && !isNullOrEmpty(scheduleCell.getGroup().name())
                && !isNullOrEmpty(scheduleCell.getGroup().location())
                && scheduleCell.getGroup().maxParticipants() > MIN_PARTICIPANTS;
    }

    private boolean validWorkout() {

        return !isNull(scheduleCell.getWorkout())
                && !isNull(scheduleCell.getWorkout().groupWorkoutId())
                && !isNull(scheduleCell.getWorkout().workoutId())
                && !isNull(scheduleCell.getWorkout().startTime())
                && !isNull(scheduleCell.getWorkout().endTime())
                && scheduleCell.getWorkout().startTime().isBefore(
                        scheduleCell.getWorkout().endTime());
    }

    private boolean validTrainer() {

        return !isNull(scheduleCell.getTrainer())
                && !isNull(scheduleCell.getTrainer().trainerId())
                && !isNullOrEmpty(scheduleCell.getTrainer().name())
                && !isNullOrEmpty(scheduleCell.getTrainer().surname());
    }

    private boolean validParticipants() {

        return !isNull(scheduleCell.getParticipants());
    }

    private boolean validPermissions() {

        return !isNull(scheduleCell.getPermissions());
    }
}
