package groups.groupPermission.controller;

import groups.common.innerCommunicators.resolver.ResolvedStatus;
import groups.common.validation.ValidationError;
import groups.common.wrappers.ValidationErrors;
import groups.group.repository.GroupQuery;
import groups.groupPermission.controller.form.GroupPermissionForm;
import groups.groupPermission.permission.PermissionValidator;
import groups.groupPermission.repository.GroupPermissionQuery;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Service;
import org.springframework.util.Assert;

import java.util.UUID;

import static groups.common.innerCommunicators.resolver.InnerCommunicationStatusResolver.resolveIdCheckStatus;
import static org.springframework.http.HttpStatus.*;

@Service
public class GroupPermissionValidator {

    private final GroupQuery groupQuery;
    private final GroupPermissionQuery groupPermissionQuery;
    private final PermissionValidator permissionValidator;


    @Autowired
    private GroupPermissionValidator(GroupQuery groupQuery,
                                     GroupPermissionQuery groupPermissionQuery,
                                     PermissionValidator permissionValidator) {

        Assert.notNull(groupQuery, "groupQuery must not be null");
        Assert.notNull(groupPermissionQuery, "workoutPermissionQuery must not be null");
        Assert.notNull(permissionValidator, "permissionValidator must not be null");

        this.groupQuery = groupQuery;
        this.groupPermissionQuery = groupPermissionQuery;
        this.permissionValidator = permissionValidator;
    }


    void validateBeforeAdd(GroupPermissionForm groupPermissionForm, ValidationErrors errors) {

        Assert.notNull(groupPermissionForm, "workoutPermissionForm must not be null");
        Assert.notNull(errors, "errors must not be null");

        checkGroupWorkoutId(groupPermissionForm.groupId(), errors);
        checkPermissionId(groupPermissionForm.permissionId(), errors);
        checkIfAssigned(groupPermissionForm.groupId(), groupPermissionForm.permissionId(), errors);
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

        if (groupQuery.findGroupById(groupWorkoutId).isEmpty()) {

            errors.addError(new ValidationError(BAD_REQUEST, "GroupWorkout with given ID does not exist"));
        }
    }

    private void checkGroupPermissionId(UUID id, ValidationErrors errors) {

        if (groupPermissionQuery.findGroupPermissionById(id).isEmpty()) {

            errors.addError(new ValidationError(BAD_REQUEST, "GroupPermission with given ID does not exist"));
        }
    }

    private void checkIfAssigned(UUID groupWorkoutId, UUID permissionId, ValidationErrors errors) {

        if (!groupPermissionQuery.getAllGroupPermissionsByGroupIdAndPermissionId(groupWorkoutId, permissionId).isEmpty()) {

            errors.addError(new ValidationError(BAD_REQUEST, "Given permissionID is already assigned to this groupWorkoutID"));
        }
    }

    private void checkIfNotAssigned(UUID groupWorkoutId, UUID permissionId, ValidationErrors errors) {

        if (groupPermissionQuery.getAllGroupPermissionsByGroupIdAndPermissionId(groupWorkoutId, permissionId).isEmpty()) {

            errors.addError(new ValidationError(BAD_REQUEST, "Given permissionID is not assigned to this groupWorkoutID"));
        }
    }
}
