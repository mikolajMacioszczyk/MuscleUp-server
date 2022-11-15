package content.exercise.controller.form;

import org.springframework.lang.Nullable;

import java.util.List;
import java.util.UUID;

public record ExerciseForm(
        String name,
        String description,
        @Nullable String imageUrl,
        List<UUID> criteria
) { }