package groups.workoutParticipant.entity;

import org.springframework.util.Assert;

public class WorkoutParticipantFullDtoFactory {

    public WorkoutParticipantFullDto create(WorkoutParticipant workoutParticipant) {

        Assert.notNull(workoutParticipant, "workoutParticipant must not be null");

        return new WorkoutParticipantFullDto(
                workoutParticipant.getId(),
                workoutParticipant.getGroupWorkout().getId(),
                workoutParticipant.getGympassId()
        );
    }
}
