package groups.schedule.controller;

import groups.common.errors.ValidationError;
import groups.common.innerCommunicators.resolver.ResolvedStatus;
import groups.common.wrappers.ValidationErrors;
import groups.groupTrainer.trainer.TrainerRepository;
import groups.groupWorkout.repository.GroupWorkoutQuery;
import groups.groupWorkout.workout.WorkoutRepository;
import groups.schedule.controller.form.ScheduleCellForm;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Service;
import org.springframework.util.Assert;

import java.time.LocalDateTime;
import java.util.UUID;

import static groups.common.innerCommunicators.resolver.InnerCommunicationStatusResolver.resolveIdCheckStatus;
import static groups.common.utils.StringUtils.isNullOrEmpty;
import static groups.groupWorkout.controller.GroupWorkoutValidator.MIN_PARTICIPANTS;
import static org.springframework.http.HttpStatus.BAD_REQUEST;
import static org.springframework.http.HttpStatus.OK;

@Service
public class ScheduleValidator {

    private final GroupWorkoutQuery groupWorkoutQuery;
    private final TrainerRepository trainerRepository;
    private final WorkoutRepository workoutRepository;


    @Autowired
    public ScheduleValidator(GroupWorkoutQuery groupWorkoutQuery,
                             TrainerRepository trainerRepository,
                             WorkoutRepository workoutRepository) {

        Assert.notNull(groupWorkoutQuery, "groupWorkoutQuery must not be null");
        Assert.notNull(trainerRepository, "trainerRepository must not be null");
        Assert.notNull(workoutRepository, "workoutRepository must not be null");

        this.groupWorkoutQuery = groupWorkoutQuery;
        this.trainerRepository = trainerRepository;
        this.workoutRepository = workoutRepository;
    }


    public void validateBeforeSave(ScheduleCellForm scheduleCellForm, ValidationErrors errors) {

        Assert.notNull(scheduleCellForm, "scheduleCellForm must not be null");
        Assert.notNull(errors, "errors must not be null");

        checkFormFields(scheduleCellForm, errors);
    }

    public void validateBeforeSingleUpdate(UUID id, ScheduleCellForm scheduleCellForm, ValidationErrors errors) {

        Assert.notNull(id, "id must not be null");
        Assert.notNull(scheduleCellForm, "scheduleCellForm must not be null");
        Assert.notNull(errors, "errors must not be null");

        checkGroupWorkoutId(id, errors);
        checkFormFields(scheduleCellForm, errors);
    }

    public void validateBeforeCascadeUpdate(UUID id, ScheduleCellForm scheduleCellForm, ValidationErrors errors) {

        Assert.notNull(id, "id must not be null");
        Assert.notNull(scheduleCellForm, "scheduleCellForm must not be null");
        Assert.notNull(errors, "errors must not be null");

        checkGroupWorkoutId(id, errors);
        checkFormFields(scheduleCellForm, errors);
    }

    public void validateBeforeDelete(UUID id, ValidationErrors errors) {

        Assert.notNull(id, "id must not be null");
        Assert.notNull(errors, "errors must not be null");

        checkGroupWorkoutId(id, errors);
    }


    private void checkFormFields(ScheduleCellForm scheduleCellForm, ValidationErrors errors) {

        checkName(scheduleCellForm.name(), errors);
        checkDates(scheduleCellForm.startTime(), scheduleCellForm.endTime(), errors);
        checkoutLocation(scheduleCellForm.location(), errors);
        checkoutMaxParticipants(scheduleCellForm.maxParticipants(), errors);
        checkTrainerId(scheduleCellForm.trainerId(), errors);
        checkWorkoutId(scheduleCellForm.workoutId(), errors);
    }

    private void checkGroupWorkoutId(UUID id, ValidationErrors errors) {

        if (groupWorkoutQuery.findGroupWorkoutById(id).isEmpty()) {

            errors.addError(new ValidationError(BAD_REQUEST, "GroupWorkout with given ID does not exist"));
        }
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

    private void checkTrainerId(UUID id, ValidationErrors errors) {

        HttpStatus validationStatus = trainerRepository.checkTrainerId(id);

        ResolvedStatus resolvedStatus = resolveIdCheckStatus(validationStatus, "Trainer");

        if (resolvedStatus.httpStatus() != OK) {

            errors.addError(new ValidationError(resolvedStatus.httpStatus(), resolvedStatus.description()));
        }
    }

    private void checkWorkoutId(UUID id, ValidationErrors errors) {

        HttpStatus validationStatus = workoutRepository.checkWorkoutId(id);

        ResolvedStatus resolvedStatus = resolveIdCheckStatus(validationStatus, "Workout");

        if (resolvedStatus.httpStatus() != OK) {

            errors.addError(new ValidationError(resolvedStatus.httpStatus(), resolvedStatus.description()));
        }
    }
}
