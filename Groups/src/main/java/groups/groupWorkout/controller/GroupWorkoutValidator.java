package groups.groupWorkout.controller;

import groups.common.innerCommunicators.resolver.ResolvedStatus;
import groups.common.errors.ValidationError;
import groups.common.wrappers.ValidationErrors;
import groups.group.repository.GroupQuery;
import groups.groupWorkout.controller.form.GroupWorkoutForm;
import groups.groupWorkout.repository.GroupWorkoutQuery;
import groups.groupWorkout.workout.WorkoutRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Service;
import org.springframework.util.Assert;

import java.time.LocalDateTime;
import java.util.UUID;

import static groups.common.innerCommunicators.resolver.InnerCommunicationStatusResolver.resolveIdCheckStatus;
import static groups.common.utils.StringUtils.isNullOrEmpty;
import static org.springframework.http.HttpStatus.*;

@Service
public class GroupWorkoutValidator {

    public static final int MIN_PARTICIPANTS = 1;

    private final GroupWorkoutQuery groupWorkoutQuery;
    private final GroupQuery groupQuery;
    private final WorkoutRepository workoutRepository;


    @Autowired
    private GroupWorkoutValidator(GroupWorkoutQuery groupWorkoutQuery,
                                  GroupQuery groupQuery,
                                  WorkoutRepository workoutRepository) {

        Assert.notNull(groupWorkoutQuery, "groupWorkoutQuery must not be null");
        Assert.notNull(groupQuery, "groupQuery must not be null");
        Assert.notNull(workoutRepository, "workoutValidator must not be null");

        this.groupWorkoutQuery = groupWorkoutQuery;
        this.groupQuery = groupQuery;
        this.workoutRepository = workoutRepository;
    }


    void validateBeforeSave(GroupWorkoutForm groupWorkoutForm, ValidationErrors errors) {

        Assert.notNull(groupWorkoutForm, "groupWorkoutFullForm must not be null");
        Assert.notNull(errors, "errors must not be null");

        checkGroupId(groupWorkoutForm.groupId(), errors);
        checkWorkoutId(groupWorkoutForm.workoutId(), errors);
        checkoutLocation(groupWorkoutForm.location(), errors);
        checkoutMaxParticipants(groupWorkoutForm.maxParticipants(), errors);
        checkDates(groupWorkoutForm.startTime(), groupWorkoutForm.endTime(), errors);
    }

    void validateBeforeUpdate(UUID id, GroupWorkoutForm groupWorkoutForm, ValidationErrors errors) {

        Assert.notNull(id, "id must not be null");
        Assert.notNull(groupWorkoutForm, "groupWorkoutForm must not be null");
        Assert.notNull(errors, "errors must not be null");

        checkGroupWorkoutId(id, errors);
        checkGroupId(groupWorkoutForm.groupId(), errors);
        checkWorkoutId(groupWorkoutForm.workoutId(), errors);
        checkoutLocation(groupWorkoutForm.location(), errors);
        checkoutMaxParticipants(groupWorkoutForm.maxParticipants(), errors);
        checkDates(groupWorkoutForm.startTime(), groupWorkoutForm.endTime(), errors);
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

        HttpStatus validationStatus = workoutRepository.checkWorkoutId(id);

        ResolvedStatus resolvedStatus = resolveIdCheckStatus(validationStatus, "Workout");

        if (resolvedStatus.httpStatus() != OK) {

            errors.addError(new ValidationError(resolvedStatus.httpStatus(), resolvedStatus.description()));
        }
    }

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

    private void checkDates(LocalDateTime dateFrom, LocalDateTime dateTo, ValidationErrors errors) {

        if (!dateFrom.isBefore(dateTo)) {

            errors.addError(new ValidationError(BAD_REQUEST, "Start time can not be equal or after end time"));
        }
    }
}
