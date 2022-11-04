package groups.workoutParticipant.controller;

import groups.common.innerCommunicators.resolver.ResolvedStatus;
import groups.common.errors.ValidationError;
import groups.common.wrappers.ValidationErrors;
import groups.groupWorkout.repository.GroupWorkoutQuery;
import groups.workoutParticipant.controller.form.WorkoutParticipantForm;
import groups.workoutParticipant.participant.ParticipantValidator;
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
    private final ParticipantValidator participantValidator;


    @Autowired
    private WorkoutParticipantValidator(GroupWorkoutQuery groupWorkoutQuery,
                                        WorkoutParticipantQuery workoutParticipantQuery,
                                        ParticipantValidator participantValidator) {

        Assert.notNull(groupWorkoutQuery, "groupWorkoutQuery must not be null");
        Assert.notNull(workoutParticipantQuery, "workoutParticipantQuery must not be null");
        Assert.notNull(participantValidator, "participantValidator must not be null");

        this.groupWorkoutQuery = groupWorkoutQuery;
        this.workoutParticipantQuery = workoutParticipantQuery;
        this.participantValidator = participantValidator;
    }


    void validateBeforeAssign(WorkoutParticipantForm workoutParticipantForm, ValidationErrors errors) {

        Assert.notNull(workoutParticipantForm, "workoutParticipantForm must not be null");
        Assert.notNull(errors, "errors must not be null");

        checkGroupWorkoutId(workoutParticipantForm.groupWorkoutId(), errors);
        checkGympassId(workoutParticipantForm.gympassId(), errors);
        checkIfAssigned(workoutParticipantForm.groupWorkoutId(), workoutParticipantForm.gympassId(), errors);
    }

    void validateBeforeUnassign(UUID groupWorkoutId, UUID gympassId, ValidationErrors errors) {

        Assert.notNull(groupWorkoutId, "groupWorkoutId must not be null");
        Assert.notNull(gympassId, "gympassId must not be null");
        Assert.notNull(errors, "errors must not be null");

        checkGroupWorkoutId(groupWorkoutId, errors);
        checkGympassId(gympassId, errors);
        checkIfNotAssigned(groupWorkoutId, gympassId, errors);
    }

    void validateBeforeUnassign(UUID id, ValidationErrors errors) {

        Assert.notNull(id, "id must not be null");
        Assert.notNull(errors, "errors must not be null");

        checkWorkoutParticipantId(id, errors);
    }


    private void checkGympassId(UUID gympassId, ValidationErrors errors) {

        HttpStatus validationStatus = participantValidator.checkGympassId(gympassId);

        ResolvedStatus resolvedStatus = resolveIdCheckStatus(validationStatus, "Gympass");

        if (resolvedStatus.httpStatus() != OK) {

            errors.addError(new ValidationError(resolvedStatus.httpStatus(), resolvedStatus.description()));
        }
    }

    private void checkGroupWorkoutId(UUID groupWorkoutId, ValidationErrors errors) {

        if (groupWorkoutQuery.findGroupWorkoutById(groupWorkoutId).isEmpty()) {

            errors.addError(new ValidationError(BAD_REQUEST, "GroupWorkout with given ID does not exist"));
        }
    }

    private void checkWorkoutParticipantId(UUID id, ValidationErrors errors) {

        if (workoutParticipantQuery.findWorkoutParticipantById(id).isEmpty()) {

            errors.addError(new ValidationError(BAD_REQUEST, "WorkoutParticipant with given ID does not exist"));
        }
    }

    private void checkIfAssigned(UUID groupWorkoutId, UUID gympassId, ValidationErrors errors) {

        if (!workoutParticipantQuery.getAllWorkoutParticipantsByGroupWorkoutIdAndGympassId(groupWorkoutId, gympassId).isEmpty()) {

            errors.addError(new ValidationError(BAD_REQUEST, "Given gympassID is already assigned to this groupWorkoutID"));
        }
    }

    private void checkIfNotAssigned(UUID groupWorkoutId, UUID gympassId, ValidationErrors errors) {

        if (workoutParticipantQuery.getAllWorkoutParticipantsByGroupWorkoutIdAndGympassId(groupWorkoutId, gympassId).isEmpty()) {

            errors.addError(new ValidationError(BAD_REQUEST, "Given gympassID is not assigned to this groupWorkoutID"));
        }
    }
}
