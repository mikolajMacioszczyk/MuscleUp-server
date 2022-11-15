package content.performedWorkout.controller;

import content.common.errors.ValidationError;
import content.common.wrappers.ValidationErrors;
import content.exercise.repository.ExerciseQuery;
import content.performedWorkout.controller.form.PerformedWorkoutForm;
import content.workout.controller.form.CriterionValueForm;
import content.workout.controller.form.ExerciseValueForm;
import content.workout.repository.WorkoutQuery;
import content.workoutExercise.repository.WorkoutExerciseQuery;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.util.Assert;

import java.time.ZonedDateTime;
import java.util.List;
import java.util.UUID;

import static java.util.Objects.isNull;
import static org.springframework.http.HttpStatus.BAD_REQUEST;

@Service
public class PerformedWorkoutValidator {

    private final WorkoutQuery workoutQuery;
    private final WorkoutExerciseQuery workoutExerciseQuery;
    private final ExerciseQuery exerciseQuery;


    @Autowired
    public PerformedWorkoutValidator(WorkoutQuery workoutQuery,
                                     WorkoutExerciseQuery workoutExerciseQuery,
                                     ExerciseQuery exerciseQuery) {

        Assert.notNull(workoutQuery, "workoutQuery must not be null");
        Assert.notNull(workoutExerciseQuery, "workoutExerciseQuery must not be null");
        Assert.notNull(exerciseQuery, "exerciseQuery must not be null");

        this.workoutQuery = workoutQuery;
        this.workoutExerciseQuery = workoutExerciseQuery;
        this.exerciseQuery = exerciseQuery;
    }


    public void validateBeforeSave(UUID fitnessClubId, PerformedWorkoutForm form, ValidationErrors errors) {

        Assert.notNull(form, "form must not be null");
        Assert.notNull(errors, "errors must not be null");

        checkWorkoutId(form.workoutId(), fitnessClubId, errors);
        checkUserId(form.userId(), errors);
        checkTime(form.time(), errors);
        checkAll(fitnessClubId, form.exercises(), errors);
    }

    private void checkWorkoutId(UUID id, UUID fitnessClubId, ValidationErrors errors) {

        if (workoutQuery.findById(id, fitnessClubId).isEmpty()) {

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

    private void checkAll(UUID fitnessClubId, List<ExerciseValueForm> exercises, ValidationErrors errors) {

        checkWorkoutExercises(exercises, errors);

        if (errors.hasErrors()) return;

        checkExercises(fitnessClubId, exercises, errors);

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

    private void checkWorkoutExercises(List<ExerciseValueForm> exercises, ValidationErrors errors) {

        exercises.forEach(exercise -> {

            if (isNull(exercise.workoutExerciseId())) {

                errors.addError(new ValidationError(BAD_REQUEST, "WorkoutExerciseId can not be null here"));
            }
        });

        if (errors.hasErrors()) return;

        exercises.forEach(exercise -> {

            if (workoutExerciseQuery.findById(exercise.workoutExerciseId()).isEmpty()) {

                errors.addError(new ValidationError(BAD_REQUEST, "WorkoutExerciseId: " + exercise.workoutExerciseId() + " doesn't exist"));
            }
        });
    }

    private void checkExercises(UUID fitnessClubId, List<ExerciseValueForm> exercises, ValidationErrors errors) {

        exercises.forEach(exercise -> {

            if (exerciseQuery.findById(exercise.exerciseId(), fitnessClubId).isEmpty()) {

                errors.addError(new ValidationError(BAD_REQUEST, "ExerciseId: " + exercise.exerciseId() + " doesn't exist"));
            }
        });
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
