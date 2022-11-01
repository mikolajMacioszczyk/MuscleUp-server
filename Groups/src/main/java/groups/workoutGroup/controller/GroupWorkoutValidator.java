package groups.workoutGroup.controller;

import groups.common.innerCommunicators.resolver.ResolvedStatus;
import groups.common.validation.ValidationError;
import groups.common.wrappers.ValidationErrors;
import groups.group.repository.GroupQuery;
import groups.workoutGroup.controller.form.GroupWorkoutFullForm;
import groups.workoutGroup.entity.GroupWorkoutFullDto;
import groups.workoutGroup.repository.GroupWorkoutQuery;
import groups.workoutGroup.workout.WorkoutValidator;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Service;
import org.springframework.util.Assert;

import java.time.LocalDateTime;
import java.util.UUID;

import static groups.common.innerCommunicators.resolver.InnerCommunicationStatusResolver.resolveIdCheckStatus;
import static org.springframework.http.HttpStatus.*;

@Service
public class GroupWorkoutValidator {

    private final GroupWorkoutQuery groupWorkoutQuery;
    private final GroupQuery groupQuery;
    private final WorkoutValidator workoutValidator;


    @Autowired
    private GroupWorkoutValidator(GroupWorkoutQuery groupWorkoutQuery,
                                  GroupQuery groupQuery,
                                  WorkoutValidator workoutValidator) {

        Assert.notNull(groupWorkoutQuery, "groupWorkoutQuery must not be null");
        Assert.notNull(groupQuery, "groupQuery must not be null");
        Assert.notNull(workoutValidator, "workoutValidator must not be null");

        this.groupWorkoutQuery = groupWorkoutQuery;
        this.groupQuery = groupQuery;
        this.workoutValidator = workoutValidator;
    }


    // TODO czy nie ma groupWorkout z workoutId i groupId w tych godzinach
    void validateBeforeSave(GroupWorkoutFullForm groupWorkoutFullForm, ValidationErrors errors) {

        Assert.notNull(groupWorkoutFullForm, "groupWorkoutFullForm must not be null");
        Assert.notNull(errors, "errors must not be null");

        checkGroupId(groupWorkoutFullForm.groupId(), errors);
        checkWorkoutId(groupWorkoutFullForm.workoutId(), errors);
        checkDates(groupWorkoutFullForm.startTime(), groupWorkoutFullForm.endTime(), errors);
    }

    void validateBeforeUpdate(GroupWorkoutFullDto groupWorkoutFullDto, ValidationErrors errors) {

        Assert.notNull(groupWorkoutFullDto, "groupWorkoutFullDto must not be null");
        Assert.notNull(errors, "errors must not be null");

        checkGroupWorkoutId(groupWorkoutFullDto.id(), errors);
        checkGroupId(groupWorkoutFullDto.groupId(), errors);
        checkWorkoutId(groupWorkoutFullDto.workoutId(), errors);
        checkDates(groupWorkoutFullDto.startTime(), groupWorkoutFullDto.endTime(), errors);
    }

    void validateBeforeDelete(UUID id, ValidationErrors errors) {

        Assert.notNull(id, "id must not be null");
        Assert.notNull(errors, "errors must not be null");

        checkGroupWorkoutId(id, errors);
    }


    private void checkGroupWorkoutId(UUID id, ValidationErrors errors) {

        if (groupWorkoutQuery.findGroupWorkoutById(id).isEmpty()) {

            errors.addError(new ValidationError(BAD_REQUEST, "GroupWorkout with given ID does not exist"));
        }
    }

    private void checkGroupId(UUID id, ValidationErrors errors) {

        if (groupQuery.findGroupById(id).isEmpty()) {

            errors.addError(new ValidationError(BAD_REQUEST, "Group with given ID does not exist"));
        }
    }

    private void checkWorkoutId(UUID id, ValidationErrors errors) {

        HttpStatus validationStatus = workoutValidator.checkWorkoutId(id);

        ResolvedStatus resolvedStatus = resolveIdCheckStatus(validationStatus, "Workout");

        if (resolvedStatus.httpStatus() != OK) {

            errors.addError(new ValidationError(resolvedStatus.httpStatus(), resolvedStatus.description()));
        }
    }

    private void checkDates(LocalDateTime dateFrom, LocalDateTime dateTo, ValidationErrors errors) {

        if (!dateFrom.isBefore(dateTo)) {

            errors.addError(new ValidationError(BAD_REQUEST, "Start time can not be equal or after end time"));
        }
    }
}
