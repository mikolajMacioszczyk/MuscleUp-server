package groups.workoutPermission.controller;

import groups.common.innerCommunicators.resolver.ResolvedStatus;
import groups.common.validation.ValidationError;
import groups.common.wrappers.ValidationErrors;
import groups.workoutGroup.repository.GroupWorkoutQuery;
import groups.workoutPermission.controller.form.WorkoutPermissionForm;
import groups.workoutPermission.permission.PermissionValidator;
import groups.workoutPermission.repository.WorkoutPermissionQuery;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Service;
import org.springframework.util.Assert;

import java.util.UUID;

import static groups.common.innerCommunicators.resolver.InnerCommunicationStatusResolver.resolveIdCheckStatus;
import static org.springframework.http.HttpStatus.*;

@Service
public class WorkoutPermissionValidator {

    private final GroupWorkoutQuery groupWorkoutQuery;
    private final WorkoutPermissionQuery workoutPermissionQuery;
    private final PermissionValidator permissionValidator;


    @Autowired
    private WorkoutPermissionValidator(GroupWorkoutQuery groupWorkoutQuery,
                                       WorkoutPermissionQuery workoutPermissionQuery,
                                       PermissionValidator permissionValidator) {

        Assert.notNull(groupWorkoutQuery, "groupWorkoutQuery must not be null");
        Assert.notNull(workoutPermissionQuery, "workoutPermissionQuery must not be null");
        Assert.notNull(permissionValidator, "permissionValidator must not be null");

        this.groupWorkoutQuery = groupWorkoutQuery;
        this.workoutPermissionQuery = workoutPermissionQuery;
        this.permissionValidator = permissionValidator;
    }


    void validateBeforeAdd(WorkoutPermissionForm workoutPermissionForm, ValidationErrors errors) {

        Assert.notNull(workoutPermissionForm, "workoutPermissionForm must not be null");
        Assert.notNull(errors, "errors must not be null");

        checkGroupWorkoutId(workoutPermissionForm.groupWorkoutId(), errors);
        checkPermissionId(workoutPermissionForm.permissionId(), errors);
        checkIfAssigned(workoutPermissionForm.groupWorkoutId(), workoutPermissionForm.permissionId(), errors);
    }

    void validateBeforeRemove(UUID groupWorkoutId, UUID permissionId, ValidationErrors errors) {

        Assert.notNull(groupWorkoutId, "groupWorkoutId must not be null");
        Assert.notNull(permissionId, "permissionId must not be null");
        Assert.notNull(errors, "errors must not be null");

        checkGroupWorkoutId(groupWorkoutId, errors);
        checkPermissionId(permissionId, errors);
        checkIfNotAssigned(groupWorkoutId, permissionId, errors);
    }

    void validateBeforeRemove(UUID id, ValidationErrors errors) {

        Assert.notNull(id, "id must not be null");
        Assert.notNull(errors, "errors must not be null");

        checkGroupPermissionId(id, errors);
    }


    private void checkPermissionId(UUID permissionId, ValidationErrors errors) {

        HttpStatus validationStatus = permissionValidator.checkPermissionId(permissionId);

        ResolvedStatus resolvedStatus = resolveIdCheckStatus(validationStatus, "Permission");

        if (resolvedStatus.httpStatus() != OK) {

            errors.addError(new ValidationError(resolvedStatus.httpStatus(), resolvedStatus.description()));
        }
    }

    private void checkGroupWorkoutId(UUID groupWorkoutId, ValidationErrors errors) {

        if (groupWorkoutQuery.findGroupWorkoutById(groupWorkoutId).isEmpty()) {

            errors.addError(new ValidationError(BAD_REQUEST, "GroupWorkout with given ID does not exist"));
        }
    }

    private void checkGroupPermissionId(UUID id, ValidationErrors errors) {

        if (workoutPermissionQuery.findWorkoutPermissionById(id).isEmpty()) {

            errors.addError(new ValidationError(BAD_REQUEST, "GroupPermission with given ID does not exist"));
        }
    }

    private void checkIfAssigned(UUID groupWorkoutId, UUID permissionId, ValidationErrors errors) {

        if (!workoutPermissionQuery.getAllWorkoutPermissionsByGroupWorkoutIdAndPermissionId(groupWorkoutId, permissionId).isEmpty()) {

            errors.addError(new ValidationError(BAD_REQUEST, "Given permissionID is already assigned to this groupWorkoutID"));
        }
    }

    private void checkIfNotAssigned(UUID groupWorkoutId, UUID permissionId, ValidationErrors errors) {

        if (workoutPermissionQuery.getAllWorkoutPermissionsByGroupWorkoutIdAndPermissionId(groupWorkoutId, permissionId).isEmpty()) {

            errors.addError(new ValidationError(BAD_REQUEST, "Given permissionID is not assigned to this groupWorkoutID"));
        }
    }
}
