package content.exercise.entity;

import content.criterion.entity.CriterionDto;
import content.exercise.controller.form.ExerciseForm;
import org.springframework.lang.Nullable;

import java.util.List;
import java.util.Objects;
import java.util.UUID;
import java.util.concurrent.atomic.AtomicBoolean;

public record ExerciseDto(
        UUID id,
        UUID fitnessClubId,
        String name,
        String description,
        @Nullable String imageUrl,
        List<CriterionDto> criteria
) {

    public boolean isSoftEditNeeded(ExerciseForm form, UUID formFitnessClubId) {

        return hasEditablePartChanged(form)
                && !haveCriteriaChanged(form)
                && fitnessClubId.equals(formFitnessClubId);
    }

    public boolean isHardEditNeeded(ExerciseForm form, UUID formFitnessClubId) {

        return haveCriteriaChanged(form)
                || !fitnessClubId.equals(formFitnessClubId);
    }


    private boolean hasEditablePartChanged(ExerciseForm form) {

        return !Objects.equals(name, form.name())
                || !Objects.equals(description, form.description())
                || imageUrl != imageUrl();
    }

    private boolean haveCriteriaChanged(ExerciseForm form) {

        return criteria.size() != form.criteria().size()
                || compareCriteria(form.criteria());
    }

    private boolean compareCriteria(List<UUID> formCriteria) {

        AtomicBoolean result = new AtomicBoolean(false);

        formCriteria.forEach(id -> {

            if (!criteria.stream().map(CriterionDto::id).toList().contains(id)) {

                result.set(true);
            }

        });

        return result.get();
    }
}