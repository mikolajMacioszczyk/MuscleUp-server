package groups.group.controller;

import groups.common.errors.ValidationError;
import groups.common.wrappers.ValidationErrors;
import groups.group.controller.form.GroupForm;
import groups.group.repository.GroupQuery;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.util.Assert;

import java.time.LocalDateTime;
import java.util.UUID;

import static groups.common.utils.StringUtils.isNullOrEmpty;
import static org.springframework.http.HttpStatus.BAD_REQUEST;

@Service
public class GroupValidator {

    private final GroupQuery groupQuery;


    @Autowired
    private GroupValidator(GroupQuery groupQuery) {

        Assert.notNull(groupQuery, "groupQuery must not be null");

        this.groupQuery = groupQuery;
    }


    void validateBeforeSave(GroupForm groupForm, ValidationErrors errors) {

        Assert.notNull(groupForm, "groupFullForm must not be null");
        Assert.notNull(errors, "errors must not be null");

        checkName(groupForm.name(), errors);
    }

    void validateBeforeUpdate(UUID id, GroupForm groupForm, ValidationErrors errors) {

        Assert.notNull(id, "id must not be null");
        Assert.notNull(groupForm, "groupForm must not be null");
        Assert.notNull(errors, "errors must not be null");

        checkGroupId(id, errors);
        checkName(groupForm.name(), errors);
    }

    void validateBeforeDelete(UUID id, ValidationErrors errors) {

        Assert.notNull(id, "id must not be null");
        Assert.notNull(errors, "errors must not be null");

        checkGroupId((id), errors);
    }


    private void checkName(String name, ValidationErrors errors) {

        if (isNullOrEmpty(name)) {

            errors.addError(new ValidationError(BAD_REQUEST, "Group name can not be empty"));
        }
    }

    private void checkGroupId(UUID id, ValidationErrors errors) {

        if (groupQuery.findGroupById(id).isEmpty()) {

            errors.addError(new ValidationError(BAD_REQUEST, "Group with given ID does not exist"));
        }
    }
}
