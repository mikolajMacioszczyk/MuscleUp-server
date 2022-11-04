package groups.groupWorkout.controller;

import groups.common.innerCommunicators.resolver.ResolvedStatus;
import groups.common.errors.ValidationError;
import groups.common.wrappers.ValidationErrors;
import groups.group.repository.GroupQuery;
import groups.groupWorkout.controller.form.GroupWorkoutFullForm;
import groups.groupWorkout.entity.GroupWorkoutFullDto;
import groups.groupWorkout.repository.GroupWorkoutQuery;
import groups.groupWorkout.workout.WorkoutValidator;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Service;
import org.springframework.util.Assert;

import java.util.UUID;

import static groups.common.innerCommunicators.resolver.InnerCommunicationStatusResolver.resolveIdCheckStatus;
import static groups.common.utils.StringUtils.isNullOrEmpty;
import static org.springframework.http.HttpStatus.*;

@Service
public class GroupWorkoutValidator {

    public static final int MIN_PARTICIPANTS = 1;

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


    void validateBeforeSave(GroupWorkoutFullForm groupWorkoutFullForm, ValidationErrors errors) {

        Assert.notNull(groupWorkoutFullForm, "groupWorkoutFullForm must not be null");
        Assert.notNull(errors, "errors must not be null");

        checkGroupId(groupWorkoutFullForm.groupId(), errors);
        checkWorkoutId(groupWorkoutFullForm.workoutId(), errors);
        checkoutLocation(groupWorkoutFullForm.location(), errors);
        checkoutMaxParticipants(groupWorkoutFullForm.maxParticipants(), errors);
    }

    void validateBeforeUpdate(GroupWorkoutFullDto groupWorkoutFullDto, ValidationErrors errors) {

        Assert.notNull(groupWorkoutFullDto, "groupWorkoutFullDto must not be null");
        Assert.notNull(errors, "errors must not be null");

        checkGroupWorkoutId(groupWorkoutFullDto.id(), errors);
        checkGroupId(groupWorkoutFullDto.groupId(), errors);
        checkWorkoutId(groupWorkoutFullDto.workoutId(), errors);
        checkoutLocation(groupWorkoutFullDto.location(), errors);
        checkoutMaxParticipants(groupWorkoutFullDto.maxParticipants(), errors);
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

    // TODO dodać poprawny agregat dla lokalizacji np. sala, piętro, opis
    private void checkoutLocation(String location, ValidationErrors errors) {

        if (isNullOrEmpty(location)) {

            errors.addError(new ValidationError(BAD_REQUEST, "GroupWorkout localization can not be empty"));
        }
    }

    private void checkoutMaxParticipants(int maxParticipants, ValidationErrors errors) {

        if (maxParticipants < MIN_PARTICIPANTS) {

            errors.addError(new ValidationError(BAD_REQUEST, "GroupWorkout max participant limit can not be below " + MIN_PARTICIPANTS));
        }
    }
}
