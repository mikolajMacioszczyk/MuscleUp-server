package content.workout.controller.form;

import content.workout.entity.WorkoutComparisonDto;
import org.springframework.lang.Nullable;

import java.util.List;
import java.util.Objects;
import java.util.UUID;
import java.util.concurrent.atomic.AtomicBoolean;

public record WorkoutForm (
        UUID creatorId,
        String description,
        @Nullable String videoUrl,
        List<UUID> bodyParts,
        List<UUID> exercises
) {

    public boolean isSimpleChange(WorkoutComparisonDto original) {

        return creatorId == original.creatorId()
                && !hasIndividualFieldsChanged(original)
                && (
                        !Objects.equals(description, original.description())
                        || videoUrl != original.videoUrl()
                );
    }

    public boolean hasChanged(WorkoutComparisonDto original) {

        return creatorId != original.creatorId()
                || !Objects.equals(description, original.description())
                || hasIndividualFieldsChanged(original);

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

        if (exercises.size() != original.exercises().size()) {

            return true;
        }

        for(int i=0; i<exercises.size(); i++) {

            if(exercises.get(i) != original.exercises().get(i)) {

                return true;
            }
        }

        return false;
    }
}
