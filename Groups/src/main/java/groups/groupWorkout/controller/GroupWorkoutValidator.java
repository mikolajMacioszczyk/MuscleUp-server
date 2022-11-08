package groups.groupWorkout.controller;

import groups.common.innerCommunicators.resolver.ResolvedStatus;
import groups.common.errors.ValidationError;
import groups.common.wrappers.ValidationErrors;
import groups.group.repository.GroupQuery;
import groups.groupWorkout.controller.form.GroupWorkoutForm;
import groups.groupWorkout.repository.GroupWorkoutQuery;
import groups.groupWorkout.workout.WorkoutQuery;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Service;
import org.springframework.util.Assert;

import java.time.ZonedDateTime;
import java.util.UUID;

import static groups.common.innerCommunicators.resolver.InnerCommunicationStatusResolver.resolveIdCheckStatus;
import static org.springframework.http.HttpStatus.*;

@Service
public class GroupWorkoutValidator {

    private final GroupWorkoutQuery groupWorkoutQuery;
    private final GroupQuery groupQuery;
    private final WorkoutQuery workoutQuery;


    @Autowired
    private GroupWorkoutValidator(GroupWorkoutQuery groupWorkoutQuery,
                                  GroupQuery groupQuery,
                                  WorkoutQuery workoutQuery) {

        Assert.notNull(groupWorkoutQuery, "groupWorkoutQuery must not be null");
        Assert.notNull(groupQuery, "groupQuery must not be null");
        Assert.notNull(workoutQuery, "workoutValidator must not be null");

        this.groupWorkoutQuery = groupWorkoutQuery;
        this.groupQuery = groupQuery;
        this.workoutQuery = workoutQuery;
    }


    void validateBeforeSave(GroupWorkoutForm groupWorkoutForm, ValidationErrors errors) {

        Assert.notNull(groupWorkoutForm, "groupWorkoutFullForm must not be null");
        Assert.notNull(errors, "errors must not be null");

        checkFields(groupWorkoutForm, errors);
    }

    void validateBeforeUpdate(UUID id, GroupWorkoutForm groupWorkoutForm, ValidationErrors errors) {

        Assert.notNull(id, "id must not be null");
        Assert.notNull(groupWorkoutForm, "groupWorkoutForm must not be null");
        Assert.notNull(errors, "errors must not be null");

        checkGroupWorkoutId(id, errors);
        checkFields(groupWorkoutForm, errors);
    }

    void validateBeforeDelete(UUID id, ValidationErrors errors) {

        Assert.notNull(id, "id must not be null");
        Assert.notNull(errors, "errors must not be null");

        checkGroupWorkoutId(id, errors);
    }


    public void checkFields(GroupWorkoutForm groupWorkoutForm, ValidationErrors errors) {

        checkGroupId(groupWorkoutForm.groupId(), errors);
        checkWorkoutId(groupWorkoutForm.workoutId(), errors);
        checkDates(groupWorkoutForm.startTime(), groupWorkoutForm.endTime(), errors);
    }


    public void checkGroupWorkoutId(UUID id, ValidationErrors errors) {

        if (groupWorkoutQuery.findGroupWorkoutById(id).isEmpty()) {

            errors.addError(new ValidationError(BAD_REQUEST, "GroupWorkout with given ID does not exist"));
        }
    }

    public void checkGroupId(UUID id, ValidationErrors errors) {

        if (groupQuery.findGroupById(id).isEmpty()) {

            errors.addError(new ValidationError(BAD_REQUEST, "Group with given ID does not exist"));
        }
    }

    public void checkWorkoutId(UUID id, ValidationErrors errors) {

        HttpStatus validationStatus = workoutQuery.checkWorkoutId(id);

        ResolvedStatus resolvedStatus = resolveIdCheckStatus(validationStatus, "Workout");

        if (resolvedStatus.httpStatus() != OK) {

            errors.addError(new ValidationError(resolvedStatus.httpStatus(), resolvedStatus.description()));
        }
    }

    public void checkDates(ZonedDateTime dateFrom, ZonedDateTime dateTo, ValidationErrors errors) {

        if (!dateFrom.isBefore(dateTo)) {

            errors.addError(new ValidationError(BAD_REQUEST, "Start time can not be equal or after end time"));
        }
    }
}
