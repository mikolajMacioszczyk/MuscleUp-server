package content.workout.controller.form;

import content.workout.entity.WorkoutComparisonDto;

import java.util.List;
import java.util.UUID;
import java.util.concurrent.atomic.AtomicBoolean;

import static java.util.Objects.isNull;

public record WorkoutForm (
        UUID creatorId,
        String description,
        String name,
        List<UUID> bodyParts,
        List<ExerciseValueForm> exercises
) {

    public boolean sameCreator(WorkoutComparisonDto original) {

        return creatorId.equals(original.creatorId());
    }

    public boolean isSimpleChange(WorkoutComparisonDto original) {

        return !hasIndividualFieldsChanged(original);
    }

    private boolean hasIndividualFieldsChanged(WorkoutComparisonDto original) {

        return hasBodyPartsChanged(original)
                || hasExercisesChanged(original);
    }

    private boolean hasBodyPartsChanged(WorkoutComparisonDto original) {

        if (bodyParts.size() != original.bodyParts().size()) {

            return true;
        }

        AtomicBoolean result = new AtomicBoolean(false);

        bodyParts.forEach(bodyPartId -> {

            if (!original.bodyParts().contains(bodyPartId)) {

                result.set(true);
            }
        });

        return result.get();
    }

    private boolean hasExercisesChanged(WorkoutComparisonDto original) {

        if (exercises.size() != original.workoutExercises().size()) return true;

        return isNewExerciseAdded();
    }

    private boolean isNewExerciseAdded() {

        for (ExerciseValueForm exercise : exercises) {

            if (isNull(exercise.workoutExerciseId())) {

                return true;
            }
        }

        return false;
    }
}
