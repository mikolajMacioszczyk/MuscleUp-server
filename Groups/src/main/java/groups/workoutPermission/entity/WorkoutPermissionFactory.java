package groups.workoutPermission.entity;

import groups.workout.repository.GroupWorkoutQuery;
import groups.workoutPermission.controller.form.WorkoutPermissionForm;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Component;
import org.springframework.util.Assert;

@Component
public class WorkoutPermissionFactory {

    private final GroupWorkoutQuery groupWorkoutQuery;


    @Autowired
    private WorkoutPermissionFactory(GroupWorkoutQuery groupWorkoutQuery) {

        Assert.notNull(groupWorkoutQuery, "groupWorkoutQuery must not be null");

        this.groupWorkoutQuery = groupWorkoutQuery;
    }


    public WorkoutPermission create(WorkoutPermissionForm workoutPermissionForm) {

        Assert.notNull(workoutPermissionForm, "workoutPermissionForm must not be null");

        return new WorkoutPermission(
                groupWorkoutQuery.getById(workoutPermissionForm.groupWorkoutId()),
                workoutPermissionForm.permissionId()
        );
    }
}
