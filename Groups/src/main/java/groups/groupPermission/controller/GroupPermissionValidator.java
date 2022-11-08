package groups.groupPermission.controller;

import groups.common.innerCommunicators.resolver.ResolvedStatus;
import groups.common.errors.ValidationError;
import groups.common.wrappers.ValidationErrors;
import groups.group.repository.GroupQuery;
import groups.groupPermission.controller.form.GroupPermissionForm;
import groups.groupPermission.permission.PermissionQuery;
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
    private final PermissionQuery permissionQuery;


    @Autowired
    private GroupPermissionValidator(GroupQuery groupQuery,
                                     GroupPermissionQuery groupPermissionQuery,
                                     PermissionQuery permissionQuery) {

        Assert.notNull(groupQuery, "groupQuery must not be null");
        Assert.notNull(groupPermissionQuery, "workoutPermissionQuery must not be null");
        Assert.notNull(permissionQuery, "permissionValidator must not be null");

        this.groupQuery = groupQuery;
        this.groupPermissionQuery = groupPermissionQuery;
        this.permissionQuery = permissionQuery;
    }


    void validateBeforeAssign(GroupPermissionForm form, ValidationErrors errors) {

        Assert.notNull(form, "form must not be null");
        Assert.notNull(errors, "errors must not be null");

        checkGroupId(form.groupId(), errors);
        checkPermissionId(form.permissionId(), errors);
        checkIfAssigned(form.groupId(), form.permissionId(), errors);
    }

    void validateBeforeUnassign(GroupPermissionForm form, ValidationErrors errors) {

        Assert.notNull(form, "form must not be null");
        Assert.notNull(errors, "errors must not be null");

        checkGroupId(form.groupId(), errors);
        checkPermissionId(form.permissionId(), errors);
        checkIfNotAssigned(form.groupId(), form.permissionId(), errors);
    }


    private void checkPermissionId(UUID permissionId, ValidationErrors errors) {

        HttpStatus validationStatus = permissionQuery.checkPermissionId(permissionId);

        ResolvedStatus resolvedStatus = resolveIdCheckStatus(validationStatus, "Permission");

        if (resolvedStatus.httpStatus() != OK) {

            errors.addError(new ValidationError(resolvedStatus.httpStatus(), resolvedStatus.description()));
        }
    }

    private void checkGroupId(UUID groupId, ValidationErrors errors) {

        if (groupQuery.findGroupById(groupId).isEmpty()) {

            errors.addError(new ValidationError(BAD_REQUEST, "Group with given ID does not exist"));
        }
    }

    private void checkIfAssigned(UUID groupId, UUID permissionId, ValidationErrors errors) {

        if (!groupPermissionQuery.getAllGroupPermissionsByGroupIdAndPermissionId(groupId, permissionId).isEmpty()) {

            errors.addError(new ValidationError(BAD_REQUEST, "Given permissionID is already assigned to this groupID"));
        }
    }

    private void checkIfNotAssigned(UUID groupId, UUID permissionId, ValidationErrors errors) {

        if (groupPermissionQuery.getAllGroupPermissionsByGroupIdAndPermissionId(groupId, permissionId).isEmpty()) {

            errors.addError(new ValidationError(BAD_REQUEST, "Given permissionID is not assigned to this groupID"));
        }
    }
}
