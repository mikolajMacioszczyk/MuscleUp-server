package content.exercise.entity;

public class ExerciseNameAndImageDtoFactory {

    public ExerciseNameAndImageDto create(Exercise exercise) {

        return new ExerciseNameAndImageDto(
                exercise.getName(),
                exercise.getImageUrl()
        );
    }
}
