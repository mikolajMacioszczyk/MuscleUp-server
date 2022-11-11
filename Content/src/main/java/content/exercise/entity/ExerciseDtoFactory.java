package content.exercise.entity;

import content.criterion.entity.Criterion;

public class ExerciseDtoFactory {

    public ExerciseDto create(Exercise exercise) {

        return new ExerciseDto(
                exercise.getId(),
                exercise.getName(),
                exercise.getDescription(),
                exercise.getImageUrl(),
                exercise.getVideoUrl(),
                exercise.getCriteria()
                        .stream()
                        .map(Criterion::getId)
                        .toList(),
                exercise.getCriteria()
                        .stream()
                        .map(Criterion::getName)
                        .toList()
        );
    }
}
