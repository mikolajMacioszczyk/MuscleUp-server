package groups.workout.entity;

import groups.group.repository.GroupQuery;
import groups.workout.controller.GroupWorkoutFullForm;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Component;

@Component
public class GroupWorkoutFactory {

    private final GroupQuery groupQuery;


    @Autowired
    public GroupWorkoutFactory(GroupQuery groupQuery) {

        this.groupQuery = groupQuery;
    }


    public GroupWorkout create(GroupWorkoutFullForm groupWorkoutFullForm) {

        return new GroupWorkout(
                groupWorkoutFullForm.startTime(),
                groupWorkoutFullForm.endTime(),
                groupQuery.getById(groupWorkoutFullForm.groupId()),
                groupWorkoutFullForm.workoutId()
        );
    }
}
