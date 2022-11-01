package groups.groupTrainer.controller;

import groups.common.innerCommunicators.resolver.ResolvedStatus;
import groups.common.validation.ValidationError;
import groups.common.wrappers.ValidationErrors;
import groups.group.repository.GroupQuery;
import groups.groupTrainer.controller.form.GroupTrainerForm;
import groups.groupTrainer.repository.GroupTrainerQuery;
import groups.groupTrainer.trainer.TrainerValidator;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Service;
import org.springframework.util.Assert;

import java.util.UUID;

import static groups.common.innerCommunicators.resolver.InnerCommunicationStatusResolver.resolveIdCheckStatus;
import static org.springframework.http.HttpStatus.*;

@Service
public class GroupTrainerValidator {

    private final GroupQuery groupQuery;
    private final GroupTrainerQuery groupTrainerQuery;
    private final TrainerValidator trainerValidator;


    @Autowired
    private GroupTrainerValidator(GroupQuery groupQuery,
                                  GroupTrainerQuery groupTrainerQuery,
                                  TrainerValidator trainerValidator) {

        Assert.notNull(groupQuery, "groupQuery must not be null");
        Assert.notNull(groupTrainerQuery, "groupTrainerQuery must not be null");
        Assert.notNull(trainerValidator, "trainerValidator must not be null");

        this.groupQuery = groupQuery;
        this.groupTrainerQuery = groupTrainerQuery;
        this.trainerValidator = trainerValidator;
    }


    void validateBeforeAssign(GroupTrainerForm groupTrainerForm, ValidationErrors errors) {

        Assert.notNull(groupTrainerForm, "groupTrainerForm must not be null");
        Assert.notNull(errors, "errors must not be null");

        checkTrainerId(groupTrainerForm.trainerId(), errors);
        checkGroupId(groupTrainerForm.groupId(), errors);
        checkIfAssigned(groupTrainerForm.trainerId(), groupTrainerForm.groupId(), errors);
    }

    void validateBeforeUnassign(UUID trainerId, UUID groupId, ValidationErrors errors) {

        Assert.notNull(trainerId, "trainerId must not be null");
        Assert.notNull(groupId, "groupId must not be null");
        Assert.notNull(errors, "errors must not be null");

        checkTrainerId(trainerId, errors);
        checkGroupId(groupId, errors);
        checkIfNotAssigned(trainerId, groupId, errors);
    }

    void validateBeforeUnassign(UUID id, ValidationErrors errors) {

        Assert.notNull(id, "id must not be null");

        checkGroupTrainerId(id, errors);
    }


    private void checkTrainerId(UUID id, ValidationErrors errors) {

        HttpStatus validationStatus = trainerValidator.checkTrainerId(id);

        ResolvedStatus resolvedStatus = resolveIdCheckStatus(validationStatus, "Trainer");

        if (resolvedStatus.httpStatus() != OK) {

            errors.addError(new ValidationError(resolvedStatus.httpStatus(), resolvedStatus.description()));
        }
    }

    private void checkGroupId(UUID id, ValidationErrors errors) {

        if (groupQuery.findGroupById(id).isEmpty()) {

            errors.addError(new ValidationError(BAD_REQUEST, "Group with given ID does not exist"));
        }
    }

    private void checkGroupTrainerId(UUID id, ValidationErrors errors) {

        if (groupTrainerQuery.findGroupTrainerById(id).isEmpty()) {

            errors.addError(new ValidationError(BAD_REQUEST, "GroupTrainer with given ID does not exist"));
        }
    }

    private void checkIfAssigned(UUID trainerId, UUID groupId, ValidationErrors errors) {

        if (!groupTrainerQuery.getAllGroupTrainerByGroupIdAndTrainerId(groupId, trainerId).isEmpty()) {

            errors.addError(new ValidationError(BAD_REQUEST, "Given trainerID is already assigned to this groupID"));
        }
    }

    private void checkIfNotAssigned(UUID trainerId, UUID groupId, ValidationErrors errors) {

        if (groupTrainerQuery.getAllGroupTrainerByGroupIdAndTrainerId(groupId, trainerId).isEmpty()) {

            errors.addError(new ValidationError(BAD_REQUEST, "Given trainerID is not assigned to this groupID"));
        }
    }
}
