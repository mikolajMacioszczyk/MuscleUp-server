package content.performedWorkout.controller;

import content.common.errors.ValidationError;
import content.common.wrappers.ValidationErrors;
import content.performedWorkout.controller.form.PerformedWorkoutForm;
import content.workout.repository.WorkoutQuery;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.util.Assert;

import java.time.ZonedDateTime;
import java.util.UUID;

import static org.springframework.http.HttpStatus.BAD_REQUEST;

@Service
public class PerformedWorkoutValidator {

    private final WorkoutQuery workoutQuery;


    @Autowired
    public PerformedWorkoutValidator(WorkoutQuery workoutQuery) {

        Assert.notNull(workoutQuery, "workoutQuery must not be null");

        this.workoutQuery = workoutQuery;
    }


    public void validateBeforeSave(PerformedWorkoutForm form, ValidationErrors errors) {

        Assert.notNull(form, "form must not be null");
        Assert.notNull(errors, "errors must not be null");

        checkWorkoutId(form.workoutId(), errors);
        checkUserId(form.userId(), errors);
        checkTime(form.time(), errors);
    }

    private void checkWorkoutId(UUID id, ValidationErrors errors) {

        if (workoutQuery.findById(id).isEmpty()) {

            errors.addError(new ValidationError(BAD_REQUEST, "Workout with ID " + id + " does not exist"));
        }
    }

    private void checkUserId(UUID id, ValidationErrors errors) {

        // TODO validate user
        if (false) {

            errors.addError(new ValidationError(BAD_REQUEST, "User with ID " + id + " does not exist"));
        }
    }

    private void checkTime(ZonedDateTime time, ValidationErrors errors) {

        if (time.isAfter(ZonedDateTime.now())) {

            errors.addError(new ValidationError(BAD_REQUEST, "Workout could not be performed in the future"));
        }
    }
}
