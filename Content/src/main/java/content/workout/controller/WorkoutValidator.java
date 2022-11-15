package content.workout.controller;

import content.bodyPart.controller.BodyPartValidator;
import content.common.errors.ValidationError;
import content.common.wrappers.ValidationErrors;
import content.exercise.controller.ExerciseValidator;
import content.exercise.repository.ExerciseQuery;
import content.performedWorkout.repository.PerformedWorkoutQuery;
import content.workout.controller.form.CriterionValueForm;
import content.workout.controller.form.ExerciseValueForm;
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
    private final ExerciseQuery exerciseQuery;
    private final PerformedWorkoutQuery performedWorkoutQuery;
    private final BodyPartValidator bodyPartValidator;
    private final ExerciseValidator exerciseValidator;


    @Autowired
    private WorkoutValidator(WorkoutQuery workoutQuery,
                             ExerciseQuery exerciseQuery,
                             PerformedWorkoutQuery performedWorkoutQuery,
                             BodyPartValidator bodyPartValidator,
                             ExerciseValidator exerciseValidator) {

        Assert.notNull(workoutQuery, "workoutQuery must not be null");
        Assert.notNull(exerciseQuery, "exerciseQuery must not be null");
        Assert.notNull(performedWorkoutQuery, "performedWorkoutQuery must not be null");
        Assert.notNull(bodyPartValidator, "bodyPartValidator must not be null");
        Assert.notNull(exerciseValidator, "exerciseValidator must not be null");

        this.workoutQuery = workoutQuery;
        this.exerciseQuery = exerciseQuery;
        this.performedWorkoutQuery = performedWorkoutQuery;
        this.bodyPartValidator = bodyPartValidator;
        this.exerciseValidator = exerciseValidator;
    }


    void validateBeforeSave(UUID fitnessClubId, WorkoutForm workoutForm, ValidationErrors errors) {

        Assert.notNull(workoutForm, "workoutForm must not be null");
        Assert.notNull(errors, "errors must not be null");

        checkFields(fitnessClubId, workoutForm, errors);
    }

    void validateBeforeUpdate(UUID id, UUID fitnessClubId, WorkoutForm workoutForm, ValidationErrors errors) {

        Assert.notNull(workoutForm, "workoutForm must not be null");
        Assert.notNull(errors, "errors must not be null");

        checkWorkoutId(id, fitnessClubId, errors);
        checkFields(fitnessClubId, workoutForm, errors);
    }

    void validateBeforeDelete(UUID id, UUID fitnessClubId, ValidationErrors errors) {

        Assert.notNull(id, "id must not be null");
        Assert.notNull(errors, "errors must not be null");

        checkWorkoutId(id, fitnessClubId, errors);
        checkWorkoutConnection(id, errors);
    }

    private void checkFields(UUID fitnessClubId, WorkoutForm workoutForm, ValidationErrors errors) {

        checkName(workoutForm.name(), errors);
        checkDescription(workoutForm.description(), errors);
        checkBodyParts(workoutForm.bodyParts(), errors);
        checkExercises(fitnessClubId, workoutForm.exercises(), errors);
    }


    private void checkName(String name, ValidationErrors errors) {

        if (isNullOrEmpty(name)) {

            errors.addError(new ValidationError(BAD_REQUEST, "Workout name can not be empty"));
        }
    }

    private void checkDescription(String description, ValidationErrors errors) {

        if (isNullOrEmpty(description)) {

            errors.addError(new ValidationError(BAD_REQUEST, "Workout description can not be empty"));
        }
    }

    private void checkWorkoutId(UUID id, UUID fitnessClubId, ValidationErrors errors) {

        if (workoutQuery.findById(id, fitnessClubId).isEmpty()) {

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

    private void checkExercises(UUID fitnessClubId, List<ExerciseValueForm> exercises, ValidationErrors errors) {

        exercises.forEach(exercise -> exerciseValidator.checkExerciseId(exercise.exerciseId(), fitnessClubId, errors));

        if (errors.hasErrors()) return;

        exercises.forEach(exercise -> checkExerciseCriteria(
                exercise.exerciseId(),
                exercise.criterionValues()
                        .stream()
                        .map(CriterionValueForm::criterionId)
                        .toList(),
                errors
                )
        );
    }

    private void checkExerciseCriteria(UUID exerciseId, List<UUID> criteria, ValidationErrors errors) {

        List<UUID> appliedCriteria = exerciseQuery.getAllAppliedCriteriaById(exerciseId);

        if (criteria.size() != appliedCriteria.size()) {

            errors.addError(new ValidationError(BAD_REQUEST, "Criteria for " + exerciseId + " don't match the model"));
        }

        appliedCriteria.forEach(criterion -> {

            if (!criteria.contains(criterion)) {

                errors.addError(new ValidationError(BAD_REQUEST, "Criteria for " + exerciseId + " don't match the model"));
            }
        });
    }
}
