package groups.schedule.dto;

import java.util.List;
import java.util.UUID;

public class ScheduleCell {

    private ScheduleGroupDto group;
    private ScheduleGroupWorkoutDto workout;
    private ScheduleTrainerDto trainer;
    private List<UUID> participants;
    private List<UUID> permissions;


    public ScheduleGroupDto getGroup() {
        return group;
    }

    public void setGroup(ScheduleGroupDto group) {
        this.group = group;
    }

    public ScheduleGroupWorkoutDto getWorkout() {
        return workout;
    }

    public void setWorkout(ScheduleGroupWorkoutDto workout) {
        this.workout = workout;
    }

    public ScheduleTrainerDto getTrainer() {
        return trainer;
    }

    public void setTrainer(ScheduleTrainerDto trainer) {
        this.trainer = trainer;
    }

    public List<UUID> getParticipants() {
        return participants;
    }

    public void setParticipants(List<UUID> participants) {
        this.participants = participants;
    }

    public List<UUID> getPermissions() {
        return permissions;
    }

    public void setPermissions(List<UUID> permissions) {
        this.permissions = permissions;
    }
}

