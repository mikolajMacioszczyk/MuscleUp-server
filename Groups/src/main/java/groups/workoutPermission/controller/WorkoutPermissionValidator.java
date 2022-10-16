package groups.workoutPermission.controller;

import groups.workout.repository.GroupWorkoutQuery;
import groups.workoutPermission.repository.WorkoutPermissionQuery;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Component;
import org.springframework.util.Assert;

import java.util.UUID;

@Component
public class WorkoutPermissionValidator {

    private final GroupWorkoutQuery groupWorkoutQuery;
    private final WorkoutPermissionQuery workoutPermissionQuery;


    @Autowired
    private WorkoutPermissionValidator(GroupWorkoutQuery groupWorkoutQuery, WorkoutPermissionQuery workoutPermissionQuery) {

        Assert.notNull(groupWorkoutQuery, "groupWorkoutQuery must not be null");
        Assert.notNull(workoutPermissionQuery, "workoutPermissionQuery must not be null");

        this.groupWorkoutQuery = groupWorkoutQuery;
        this.workoutPermissionQuery = workoutPermissionQuery;
    }


    boolean isCorrectToAdd(WorkoutPermissionForm workoutPermissionForm) {

        Assert.notNull(workoutPermissionForm, "workoutPermissionForm must not be null");

        return doesGroupWorkoutIdExist(workoutPermissionForm.groupWorkoutId())
                && doesPermissionExist(workoutPermissionForm.permissionId())
                && !isAssigned(workoutPermissionForm.groupWorkoutId(), workoutPermissionForm.permissionId());
    }

    boolean isCorrectToRemove(WorkoutPermissionForm workoutPermissionForm) {

        Assert.notNull(workoutPermissionForm, "workoutPermissionForm must not be null");

        return isAssigned(workoutPermissionForm.groupWorkoutId(), workoutPermissionForm.permissionId());
    }

    boolean isCorrectToRemove(UUID id) {

        Assert.notNull(id, "id must not be null");

        return doesGroupPermissionExist(id);
    }


    // TODO integracja z innym serwisem
    private boolean doesPermissionExist(UUID participantId) {

        return true;
    }

    private boolean doesGroupWorkoutIdExist(UUID groupWorkoutId) {

        return groupWorkoutQuery.findGroupWorkoutById(groupWorkoutId).isPresent();
    }

    private boolean doesGroupPermissionExist(UUID id) {

        return workoutPermissionQuery.findWorkoutPermissionById(id).isPresent();
    }

    private boolean isAssigned(UUID groupWorkoutId, UUID participantId) {

        return !workoutPermissionQuery.getAllWorkoutPermissionsByGroupWorkoutIdAndPermissionId(groupWorkoutId, participantId).isEmpty();
    }
}
