package groups.group.controller;

import groups.common.errors.ValidationError;
import groups.common.innerCommunicators.resolver.ResolvedStatus;
import groups.common.wrappers.ValidationErrors;
import groups.group.controller.form.GroupForm;
import groups.group.fitnessClub.FitnessClubQuery;
import groups.group.repository.GroupQuery;
import groups.group.trainer.TrainerQuery;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Service;
import org.springframework.util.Assert;

import java.util.UUID;

import static groups.common.innerCommunicators.resolver.InnerCommunicationStatusResolver.resolveIdCheckStatus;
import static groups.common.utils.StringUtils.isNullOrEmpty;
import static org.springframework.http.HttpStatus.BAD_REQUEST;
import static org.springframework.http.HttpStatus.OK;

@Service
public class GroupValidator {

    public static final int MIN_PARTICIPANTS = 1;

    private final GroupQuery groupQuery;
    private final TrainerQuery trainerQuery;
    private final FitnessClubQuery fitnessClubQuery;


    @Autowired
    private GroupValidator(GroupQuery groupQuery, TrainerQuery trainerQuery, FitnessClubQuery fitnessClubQuery) {

        Assert.notNull(groupQuery, "groupQuery must not be null");
        Assert.notNull(trainerQuery, "trainerQuery must not be null");
        Assert.notNull(fitnessClubQuery, "fitnessClubQuery must not be null");

        this.groupQuery = groupQuery;
        this.trainerQuery = trainerQuery;
        this.fitnessClubQuery = fitnessClubQuery;
    }

    void validateBeforeSave(GroupForm groupForm, ValidationErrors errors) {

        Assert.notNull(groupForm, "groupFullForm must not be null");
        Assert.notNull(errors, "errors must not be null");

        validateFields(groupForm, errors);
    }

    void validateBeforeUpdate(UUID id, GroupForm groupForm, ValidationErrors errors) {

        Assert.notNull(id, "id must not be null");
        Assert.notNull(groupForm, "groupForm must not be null");
        Assert.notNull(errors, "errors must not be null");

        checkGroupId(id, errors);
        validateFields(groupForm, errors);
    }

    void validateBeforeDelete(UUID id, ValidationErrors errors) {

        Assert.notNull(id, "id must not be null");
        Assert.notNull(errors, "errors must not be null");

        checkGroupId(id, errors);
    }


    public void validateFields(GroupForm groupForm, ValidationErrors errors) {

        checkName(groupForm.name(), errors);
        checkLocation(groupForm.location(), errors);
        checkMaxParticipants(groupForm.maxParticipants(), errors);
        checkTrainerId(groupForm.trainerId(), errors);
        checkFitnessClubId(groupForm.fitnessClubId(), errors);
    }

    public void checkName(String name, ValidationErrors errors) {

        if (isNullOrEmpty(name)) {

            errors.addError(new ValidationError(BAD_REQUEST, "Group name can not be empty"));
        }
    }

    public void checkLocation(String location, ValidationErrors errors) {

        if (isNullOrEmpty(location)) {

            errors.addError(new ValidationError(BAD_REQUEST, "GroupWorkout location can not be empty"));
        }
    }

    public void checkMaxParticipants(int maxParticipants, ValidationErrors errors) {

        if (maxParticipants < MIN_PARTICIPANTS) {

            errors.addError(new ValidationError(BAD_REQUEST, "GroupWorkout max participant limit can not be below " + MIN_PARTICIPANTS));
        }
    }

    private void checkGroupId(UUID id, ValidationErrors errors) {

        if (groupQuery.findGroupById(id).isEmpty()) {

            errors.addError(new ValidationError(BAD_REQUEST, "Group with given ID does not exist"));
        }
    }

    public void checkTrainerId(UUID id, ValidationErrors errors) {

        HttpStatus validationStatus = trainerQuery.checkTrainerId(id);

        ResolvedStatus resolvedStatus = resolveIdCheckStatus(validationStatus, "Trainer");

        if (resolvedStatus.httpStatus() != OK) {

            errors.addError(new ValidationError(resolvedStatus.httpStatus(), resolvedStatus.description()));
        }
    }

    public void checkFitnessClubId(UUID id, ValidationErrors errors) {

        HttpStatus validationStatus = fitnessClubQuery.checkFitnessClubId(id);

        ResolvedStatus resolvedStatus = resolveIdCheckStatus(validationStatus, "FitnessClub");

        if (resolvedStatus.httpStatus() != OK) {

            errors.addError(new ValidationError(resolvedStatus.httpStatus(), resolvedStatus.description()));
        }
    }
}
