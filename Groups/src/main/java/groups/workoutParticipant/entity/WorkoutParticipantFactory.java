package groups.workoutParticipant.entity;

import groups.groupWorkout.repository.GroupWorkoutQuery;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Component;
import org.springframework.util.Assert;

import java.util.UUID;

@Component
public class WorkoutParticipantFactory {

    private final GroupWorkoutQuery groupWorkoutQuery;


    @Autowired
    private WorkoutParticipantFactory(GroupWorkoutQuery groupWorkoutQuery) {

        Assert.notNull(groupWorkoutQuery, "groupWorkoutQuery must not be null");

        this.groupWorkoutQuery = groupWorkoutQuery;
    }


    public WorkoutParticipant create(UUID groupWorkoutId, UUID userId) {

        Assert.notNull(groupWorkoutId, "groupWorkoutId must not be null");
        Assert.notNull(userId, "userId must not be null");

        return new WorkoutParticipant(
                groupWorkoutQuery.getById(groupWorkoutId),
                userId
        );
    }
}
