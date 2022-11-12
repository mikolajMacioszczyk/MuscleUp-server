package content.workout.controller;

import content.bodyPart.controller.BodyPartValidator;
import content.common.errors.ValidationError;
import content.common.wrappers.ValidationErrors;
import content.exercise.controller.ExerciseValidator;
import content.performedWorkout.repository.PerformedWorkoutQuery;
import content.workout.controller.form.WorkoutForm;
import content.workout.repository.WorkoutQuery;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.util.Assert;

import java.util.List;
import java.util.UUID;

import static content.common.utils.StringUtils.isNullOrEmpty;
import static java.util.Collections.frequency;
import static org.springframework.http.HttpStatus.BAD_REQUEST;

@Service
public class WorkoutValidator {

    private final WorkoutQuery workoutQuery;
    private final PerformedWorkoutQuery performedWorkoutQuery;
    private final BodyPartValidator bodyPartValidator;
    private final ExerciseValidator exerciseValidator;


    @Autowired
    private WorkoutValidator(WorkoutQuery workoutQuery,
                             PerformedWorkoutQuery performedWorkoutQuery,
                             BodyPartValidator bodyPartValidator,
                             ExerciseValidator exerciseValidator) {

        Assert.notNull(workoutQuery, "workoutQuery must not be null");
        Assert.notNull(performedWorkoutQuery, "performedWorkoutQuery must not be null");
        Assert.notNull(bodyPartValidator, "bodyPartValidator must not be null");
        Assert.notNull(exerciseValidator, "exerciseValidator must not be null");

        this.workoutQuery = workoutQuery;
        this.performedWorkoutQuery = performedWorkoutQuery;
        this.bodyPartValidator = bodyPartValidator;
        this.exerciseValidator = exerciseValidator;
    }


    void validateBeforeSave(WorkoutForm workoutForm, ValidationErrors errors) {

        Assert.notNull(workoutForm, "workoutForm must not be null");
        Assert.notNull(errors, "errors must not be null");

        checkFields(workoutForm, errors);
    }

    void validateBeforeUpdate(UUID id, WorkoutForm workoutForm, ValidationErrors errors) {

        Assert.notNull(workoutForm, "workoutForm must not be null");
        Assert.notNull(errors, "errors must not be null");

        checkWorkoutId(id, errors);
        checkFields(workoutForm, errors);
    }

    void validateBeforeDelete(UUID id, ValidationErrors errors) {

        Assert.notNull(id, "id must not be null");
        Assert.notNull(errors, "errors must not be null");

        checkWorkoutId(id, errors);
        checkWorkoutConnection(id, errors);
    }

    private void checkFields(WorkoutForm workoutForm, ValidationErrors errors) {

        checkDescription(workoutForm.description(), errors);
        checkBodyParts(workoutForm.bodyParts(), errors);
        checkExercises(workoutForm.exercises(), errors);
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

    private void checkWorkoutConnection(UUID id, ValidationErrors errors) {

        if (!performedWorkoutQuery.getAllPerformedWorkoutsByWorkoutId(id).isEmpty()) {

            errors.addError(new ValidationError(BAD_REQUEST, "This workout can not be deleted. Appears in somebody's history"));
        }
    }

    private void checkBodyParts(List<UUID> bodyParts, ValidationErrors errors) {

        bodyParts.forEach(id -> bodyPartValidator.checkBodyPartId(id, errors));

        bodyParts.forEach(id -> {

            if (frequency(bodyParts, id) > 1 ) {

                errors.addError(new ValidationError(BAD_REQUEST, "Body part can be included only once"));
            }
        });
    }

    private void checkExercises(List<UUID> exercises, ValidationErrors errors) {

        exercises.forEach(id -> exerciseValidator.checkExerciseId(id, errors));
    }
}
