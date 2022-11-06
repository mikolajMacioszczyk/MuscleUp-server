package groups.groupWorkout.entity;

import groups.group.repository.GroupQuery;
import groups.groupWorkout.controller.form.GroupWorkoutForm;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.lang.Nullable;
import org.springframework.stereotype.Component;
import org.springframework.util.Assert;

import java.util.UUID;

@Component
public class GroupWorkoutFactory {

    private final GroupQuery groupQuery;


    @Autowired
    private GroupWorkoutFactory(GroupQuery groupQuery) {

        Assert.notNull(groupQuery, "groupQuery must not be null");

        this.groupQuery = groupQuery;
    }


    public GroupWorkout create(GroupWorkoutForm groupWorkoutForm) {

        Assert.notNull(groupWorkoutForm, "groupWorkoutFullForm must not be null");

        return new GroupWorkout(
                groupQuery.getById(groupWorkoutForm.groupId()),
                groupWorkoutForm.workoutId(),
                groupWorkoutForm.location(),
                groupWorkoutForm.maxParticipants(),
                groupWorkoutForm.startTime(),
                groupWorkoutForm.endTime(),
                null
        );
    }

    public GroupWorkout create(GroupWorkoutForm groupWorkoutForm, @Nullable UUID parentId) {

        Assert.notNull(groupWorkoutForm, "groupWorkoutFullForm must not be null");

        return new GroupWorkout(
                groupQuery.getById(groupWorkoutForm.groupId()),
                groupWorkoutForm.workoutId(),
                groupWorkoutForm.location(),
                groupWorkoutForm.maxParticipants(),
                groupWorkoutForm.startTime(),
                groupWorkoutForm.endTime(),
                parentId
        );
    }
}
