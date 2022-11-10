package content.workout.controller.form;

import org.springframework.lang.Nullable;

import java.util.UUID;

public record WorkoutForm (
        UUID id,
        String description,
        @Nullable String videoUrl,
        @Nullable Long expectedPerformTime
) { }