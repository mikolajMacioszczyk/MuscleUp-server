package content.exercise.repository;

import content.exercise.entity.Exercise;

import java.util.UUID;

public interface ExerciseRepository {

    Exercise getById(UUID id);

    UUID save(Exercise exercise);

    UUID update(Exercise exercise);

    void delete(UUID id);
}
