package groups.workoutParticipant.controller;

import groups.common.innerCommunicators.resolver.ResolvedStatus;
import groups.common.errors.ValidationError;
import groups.common.wrappers.ValidationErrors;
import groups.groupWorkout.repository.GroupWorkoutQuery;
import groups.workoutParticipant.controller.form.WorkoutParticipantForm;
import groups.workoutParticipant.participant.ParticipantQuery;
import groups.workoutParticipant.repository.WorkoutParticipantQuery;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Service;
import org.springframework.util.Assert;

import java.util.UUID;

import static groups.common.innerCommunicators.resolver.InnerCommunicationStatusResolver.resolveIdCheckStatus;
import static org.springframework.http.HttpStatus.*;

@Service
public class WorkoutParticipantValidator {

    private final GroupWorkoutQuery groupWorkoutQuery;
    private final WorkoutParticipantQuery workoutParticipantQuery;
    private final ParticipantQuery participantQuery;


    @Autowired
    private WorkoutParticipantValidator(GroupWorkoutQuery groupWorkoutQuery,
                                        WorkoutParticipantQuery workoutParticipantQuery,
                                        ParticipantQuery participantQuery) {

        Assert.notNull(groupWorkoutQuery, "groupWorkoutQuery must not be null");
        Assert.notNull(workoutParticipantQuery, "workoutParticipantQuery must not be null");
        Assert.notNull(participantQuery, "participantValidator must not be null");

        this.groupWorkoutQuery = groupWorkoutQuery;
        this.workoutParticipantQuery = workoutParticipantQuery;
        this.participantQuery = participantQuery;
    }


    void validateBeforeAssign(WorkoutParticipantForm form, ValidationErrors errors) {

        Assert.notNull(form, "form must not be null");
        Assert.notNull(errors, "errors must not be null");

        checkGroupWorkoutId(form.groupWorkoutId(), errors);
        if (errors.hasErrors()) return;
        checkUserId(form.userId(), form.groupWorkoutId(), errors);
        checkIfAssigned(form.groupWorkoutId(), form.userId(), errors);
    }

    void validateBeforeUnassign(WorkoutParticipantForm form, ValidationErrors errors) {

        Assert.notNull(form, "form must not be null");
        Assert.notNull(errors, "errors must not be null");

        checkGroupWorkoutId(form.groupWorkoutId(), errors);
        if (errors.hasErrors()) return;
        checkUserId(form.userId(), form.groupWorkoutId(), errors);
        checkIfNotAssigned(form.groupWorkoutId(), form.userId(), errors);
    }


    private void checkUserId(UUID userId, UUID groupWorkoutId,  ValidationErrors errors) {

        UUID fitnessClubId = groupWorkoutQuery.getFitnessClubIdByGroupWorkoutId(groupWorkoutId);

        HttpStatus validationStatus = participantQuery.checkUserId(userId, fitnessClubId);

        ResolvedStatus resolvedStatus = resolveIdCheckStatus(validationStatus, "User");

        if (resolvedStatus.httpStatus() == NOT_FOUND) {

            errors.addError(new ValidationError(NOT_FOUND, "User is not assigned to FitnessClub that organize these classes"));
        }
        else if (resolvedStatus.httpStatus() != OK) {

            errors.addError(new ValidationError(resolvedStatus.httpStatus(), resolvedStatus.description()));
        }
    }

    private void checkGroupWorkoutId(UUID groupWorkoutId, ValidationErrors errors) {

        if (groupWorkoutQuery.findGroupWorkoutById(groupWorkoutId).isEmpty()) {

            errors.addError(new ValidationError(BAD_REQUEST, "GroupWorkout with given ID does not exist"));
        }
    }

    private void checkIfAssigned(UUID groupWorkoutId, UUID userId, ValidationErrors errors) {

        if (!workoutParticipantQuery.getAllWorkoutParticipantsByGroupWorkoutIdAndUserId(groupWorkoutId, userId).isEmpty()) {

            errors.addError(new ValidationError(BAD_REQUEST, "Given userID is already assigned to this groupWorkoutID"));
        }
    }

    private void checkIfNotAssigned(UUID groupWorkoutId, UUID userId, ValidationErrors errors) {

        if (workoutParticipantQuery.getAllWorkoutParticipantsByGroupWorkoutIdAndUserId(groupWorkoutId, userId).isEmpty()) {

            errors.addError(new ValidationError(BAD_REQUEST, "Given userID is not assigned to this groupWorkoutID"));
        }
    }
}
