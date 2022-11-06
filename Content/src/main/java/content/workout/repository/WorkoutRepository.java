package content.workout.repository;

import content.workout.entity.Workout;

import java.util.UUID;

public interface WorkoutRepository {

    Workout getById(UUID id);

    UUID save(Workout workout);

    UUID update(Workout workout);

    void delete(UUID id);
}
