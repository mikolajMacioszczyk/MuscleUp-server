package content.exercise.entity;

import content.exercise.controller.form.ExerciseForm;

import static java.util.Collections.emptyList;

public class ExerciseFactory {

    public Exercise create(ExerciseForm form) {

        return new Exercise(
                form.name(),
                form.description(),
                form.imageUrl(),
                form.videoUrl(),
                true,
                emptyList()
        );
    }
}
