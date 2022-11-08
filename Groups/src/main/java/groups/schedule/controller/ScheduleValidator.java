package groups.schedule.controller;

import groups.common.errors.ValidationError;
import groups.common.utils.TimeUtils;
import groups.common.wrappers.TimeWrapper;
import groups.common.wrappers.ValidationErrors;
import groups.group.controller.GroupValidator;
import groups.groupWorkout.controller.GroupWorkoutValidator;
import groups.groupWorkout.controller.form.GroupWorkoutForm;
import groups.groupWorkout.repository.GroupWorkoutQuery;
import groups.schedule.controller.form.ScheduleCellForm;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.util.Assert;

import java.util.UUID;

import static groups.common.utils.TimeUtils.calculateTimeDiff;
import static org.springframework.http.HttpStatus.BAD_REQUEST;


@Service
public class ScheduleValidator {

    private final GroupValidator groupValidator;
    private final GroupWorkoutValidator groupWorkoutValidator;
    private final GroupWorkoutQuery groupWorkoutQuery;


    @Autowired
    public ScheduleValidator(GroupValidator groupValidator,
                             GroupWorkoutValidator groupWorkoutValidator,
                             GroupWorkoutQuery groupWorkoutQuery) {

        Assert.notNull(groupValidator, "groupValidator must not be null");
        Assert.notNull(groupWorkoutValidator, "groupWorkoutValidator must not be null");
        Assert.notNull(groupWorkoutQuery, "groupWorkoutQuery must not be null");

        this.groupValidator = groupValidator;
        this.groupWorkoutValidator = groupWorkoutValidator;
        this.groupWorkoutQuery = groupWorkoutQuery;
    }

    public void validateBeforeSave(ScheduleCellForm scheduleCellForm, ValidationErrors errors) {

        Assert.notNull(scheduleCellForm, "scheduleCellForm must not be null");
        Assert.notNull(errors, "errors must not be null");

        checkFormFields(scheduleCellForm, errors);
    }

    public void validateBeforeSaveAsCopy(UUID id, GroupWorkoutForm groupWorkoutForm, ValidationErrors errors) {

        Assert.notNull(id, "id must not be null");
        Assert.notNull(groupWorkoutForm, "groupWorkoutForm must not be null");
        Assert.notNull(errors, "errors must not be null");

        groupWorkoutValidator.checkGroupWorkoutId(id, errors);
        groupWorkoutValidator.checkFields(groupWorkoutForm, errors);
        checkTimeDiff(id, groupWorkoutForm, errors);
    }

    public void validateBeforeSingleUpdate(UUID id, ScheduleCellForm scheduleCellForm, ValidationErrors errors) {

        Assert.notNull(id, "id must not be null");
        Assert.notNull(scheduleCellForm, "scheduleCellForm must not be null");
        Assert.notNull(errors, "errors must not be null");

        groupWorkoutValidator.checkGroupWorkoutId(id, errors);
        checkFormFields(scheduleCellForm, errors);
    }

    public void validateBeforeCascadeUpdate(UUID id, ScheduleCellForm scheduleCellForm, ValidationErrors errors) {

        Assert.notNull(id, "id must not be null");
        Assert.notNull(scheduleCellForm, "scheduleCellForm must not be null");
        Assert.notNull(errors, "errors must not be null");

        groupWorkoutValidator.checkGroupWorkoutId(id, errors);
        checkFormFields(scheduleCellForm, errors);
    }

    public void validateBeforeDelete(UUID id, ValidationErrors errors) {

        Assert.notNull(id, "id must not be null");
        Assert.notNull(errors, "errors must not be null");

        groupWorkoutValidator.checkGroupWorkoutId(id, errors);
    }


    private void checkFormFields(ScheduleCellForm scheduleCellForm, ValidationErrors errors) {

        groupValidator.checkName(scheduleCellForm.name(), errors);
        groupValidator.checkLocation(scheduleCellForm.location(), errors);
        groupValidator.checkMaxParticipants(scheduleCellForm.maxParticipants(), errors);
        groupValidator.checkTrainerId(scheduleCellForm.trainerId(), errors);
        groupValidator.checkFitnessClubId(scheduleCellForm.fitnessClubId(), errors);

        groupWorkoutValidator.checkDates(scheduleCellForm.startTime(), scheduleCellForm.endTime(), errors);
        groupWorkoutValidator.checkWorkoutId(scheduleCellForm.workoutId(), errors);
    }

    private void checkTimeDiff(UUID id, GroupWorkoutForm groupWorkoutForm, ValidationErrors errors) {

        TimeWrapper originalTime = groupWorkoutQuery.getTimeById(id);

        TimeUtils.TimeDiff originalTimeDiff = calculateTimeDiff(originalTime.startTime(), originalTime.endTime());
        TimeUtils.TimeDiff formTimeDiff = calculateTimeDiff(groupWorkoutForm.startTime(), groupWorkoutForm.endTime());

        if (!originalTimeDiff.equals(formTimeDiff)) {

            errors.addError(new ValidationError(BAD_REQUEST, "GroupWorkout duration must be the same as original"));
        }
    }
}
