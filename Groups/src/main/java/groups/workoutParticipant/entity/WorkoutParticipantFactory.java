package groups.workoutParticipant.entity;

import groups.workout.repository.GroupWorkoutQuery;
import groups.workoutParticipant.controller.WorkoutParticipantForm;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Component;
import org.springframework.util.Assert;

@Component
public class WorkoutParticipantFactory {

    private final GroupWorkoutQuery groupWorkoutQuery;


    @Autowired
    public WorkoutParticipantFactory(GroupWorkoutQuery groupWorkoutQuery) {

        Assert.notNull(groupWorkoutQuery, "groupWorkoutQuery must not be null");

        this.groupWorkoutQuery = groupWorkoutQuery;
    }


    public WorkoutParticipant create(WorkoutParticipantForm workoutParticipantForm) {

        return new WorkoutParticipant(
                groupWorkoutQuery.getById(workoutParticipantForm.groupWorkoutId()),
                workoutParticipantForm.participantId()
        );
    }
}
