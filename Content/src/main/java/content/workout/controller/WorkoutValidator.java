package content.workout.controller;

import content.bodyPart.repository.BodyPartQuery;
import content.common.errors.ValidationError;
import content.common.wrappers.ValidationErrors;
import content.workout.controller.form.WorkoutForm;
import content.workout.repository.WorkoutQuery;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.util.Assert;

import java.util.UUID;

import static content.common.utils.StringUtils.isNullOrEmpty;
import static org.springframework.http.HttpStatus.BAD_REQUEST;

@Service
public class WorkoutValidator {

    private final WorkoutQuery workoutQuery;
    private final BodyPartQuery bodyPartQuery;


    @Autowired
    private WorkoutValidator(WorkoutQuery workoutQuery, BodyPartQuery bodyPartQuery) {

        Assert.notNull(workoutQuery, "workoutQuery must not be null");
        Assert.notNull(bodyPartQuery, "bodyPartQuery must not be null");

        this.workoutQuery = workoutQuery;
        this.bodyPartQuery = bodyPartQuery;
    }


    void validateBeforeSave(WorkoutForm workoutForm, ValidationErrors errors) {

        Assert.notNull(workoutForm, "workoutForm must not be null");
        Assert.notNull(errors, "errors must not be null");

        checkDescription(workoutForm.description(), errors);
    }

    void validateBeforeDelete(UUID id, ValidationErrors errors) {

        Assert.notNull(id, "id must not be null");
        Assert.notNull(errors, "errors must not be null");

        checkWorkoutId(id, errors);
    }

    void validateBeforeAddBodyPart(UUID workoutId, UUID bodyPartId, ValidationErrors errors) {

        Assert.notNull(workoutId, "workoutId must not be null");
        Assert.notNull(bodyPartId, "bodyPartId must not be null");
        Assert.notNull(errors, "errors must not be null");

        checkWorkoutId(workoutId, errors);
        checkBodyPartId(bodyPartId, errors);
        checkIfBodyPartIsNotAdded(workoutId, bodyPartId, errors);
    }

    void validateBeforeRemoveBodyPart(UUID workoutId, UUID bodyPartId, ValidationErrors errors) {

        Assert.notNull(workoutId, "workoutId must not be null");
        Assert.notNull(bodyPartId, "bodyPartId must not be null");
        Assert.notNull(errors, "errors must not be null");

        checkWorkoutId(workoutId, errors);
        checkBodyPartId(bodyPartId, errors);
        checkIfBodyPartIsAdded(workoutId, bodyPartId, errors);
    }


    private void checkDescription(String name, ValidationErrors errors) {

        if (isNullOrEmpty(name)) {

            errors.addError(new ValidationError(BAD_REQUEST, "Workout name can not be empty"));
        }
    }

    private void checkWorkoutId(UUID id, ValidationErrors errors) {

        if (workoutQuery.findById(id).isEmpty()) {

            errors.addError(new ValidationError(BAD_REQUEST, "Workout with given ID does not exist"));
        }
    }

    private void checkBodyPartId(UUID id, ValidationErrors errors) {

        if (bodyPartQuery.findById(id).isEmpty()) {

            errors.addError(new ValidationError(BAD_REQUEST, "BodyPart with given ID does not exist"));
        }
    }

    private void checkIfBodyPartIsNotAdded(UUID workoutId, UUID bodyPartId, ValidationErrors errors) {

        if (workoutQuery.getBodyPartsByWorkoutId(workoutId).contains(bodyPartId)) {

            errors.addError(new ValidationError(BAD_REQUEST, "BodyPart with given ID is already assigned to this Workout"));
        }
    }

    private void checkIfBodyPartIsAdded(UUID workoutId, UUID bodyPartId, ValidationErrors errors) {

        if (!workoutQuery.getBodyPartsByWorkoutId(workoutId).contains(bodyPartId)) {

            errors.addError(new ValidationError(BAD_REQUEST, "BodyPart with given ID is not assigned to this Workout"));
        }
    }
}
