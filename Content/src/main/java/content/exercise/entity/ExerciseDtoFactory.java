package content.exercise.entity;

import java.util.Collections;

public class ExerciseDtoFactory {

    public ExerciseDto create(Exercise exercise) {

        return new ExerciseDto(
                exercise.getId(),
                exercise.getName(),
                exercise.getDescription(),
                exercise.getImageUrl(),
                exercise.getVideoUrl(),
                Collections.emptyList()
        );
    }
}
