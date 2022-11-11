package content.exercise.entity;

import content.exercise.controller.form.ExerciseForm;
import org.springframework.lang.Nullable;

import java.util.List;
import java.util.Objects;
import java.util.UUID;
import java.util.concurrent.atomic.AtomicBoolean;

public record ExerciseDto(
        UUID id,
        String name,
        String description,
        @Nullable String imageUrl,
        @Nullable String videoUrl,
        List<UUID> criterionIds,
        List<String> criterionNames
) {

    public boolean isDifferent(ExerciseForm form) {

        return !Objects.equals(name, form.name())
                || !Objects.equals(description, form.description())
                || imageUrl != imageUrl()
                || videoUrl != videoUrl()
                || criterionIds.size() != form.criteria().size()
                || compareCriteria(form.criteria());
    }

    private boolean compareCriteria(List<UUID> criteria) {

        AtomicBoolean result = new AtomicBoolean(false);

        criteria.forEach(id -> {

            if (!criterionIds.contains(id)) result.set(true);
        });

        return result.get();
    }
}