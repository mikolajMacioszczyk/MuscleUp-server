package groups.group.controller;

import groups.common.validation.ValidationError;
import groups.common.wrappers.ValidationErrors;
import groups.group.controller.form.GroupFullForm;
import groups.group.entity.GroupFullDto;
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


    void validateBeforeSave(GroupFullForm groupFullForm, ValidationErrors errors) {

        Assert.notNull(groupFullForm, "groupFullForm must not be null");
        Assert.notNull(errors, "errors must not be null");

        checkName(groupFullForm.name(), errors);
        checkDates(groupFullForm.startTime(), groupFullForm.endTime(), errors);
    }

    void validateBeforeUpdate(GroupFullDto groupFullDto, ValidationErrors errors) {

        Assert.notNull(groupFullDto, "groupFullDto must not be null");
        Assert.notNull(errors, "errors must not be null");

        checkGroupId(groupFullDto.id(), errors);
        checkName(groupFullDto.name(), errors);
        checkDates(groupFullDto.startTime(), groupFullDto.endTime(), errors);
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

    private void checkDates(LocalDateTime dateFrom, LocalDateTime dateTo, ValidationErrors errors) {

        if (!dateFrom.isBefore(dateTo)) {

            errors.addError(new ValidationError(BAD_REQUEST, "Start time can not be equal or after end time"));
        }
    }

    private void checkGroupId(UUID id, ValidationErrors errors) {

        if (groupQuery.findGroupById(id).isEmpty()) {

            errors.addError(new ValidationError(BAD_REQUEST, "Group with given ID does not exist"));
        }
    }
}
