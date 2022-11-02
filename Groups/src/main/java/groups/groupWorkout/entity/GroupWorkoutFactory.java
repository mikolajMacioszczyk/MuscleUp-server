package groups.groupWorkout.entity;

import groups.group.repository.GroupQuery;
import groups.groupWorkout.controller.form.GroupWorkoutFullForm;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Component;
import org.springframework.util.Assert;

@Component
public class GroupWorkoutFactory {

    private final GroupQuery groupQuery;


    @Autowired
    private GroupWorkoutFactory(GroupQuery groupQuery) {

        Assert.notNull(groupQuery, "groupQuery must not be null");

        this.groupQuery = groupQuery;
    }


    public GroupWorkout create(GroupWorkoutFullForm groupWorkoutFullForm) {

        Assert.notNull(groupWorkoutFullForm, "groupWorkoutFullForm must not be null");

        return new GroupWorkout(
                groupQuery.getById(groupWorkoutFullForm.groupId()),
                groupWorkoutFullForm.workoutId(),
                groupWorkoutFullForm.location(),
                groupWorkoutFullForm.maxParticipants()
        );
    }
}