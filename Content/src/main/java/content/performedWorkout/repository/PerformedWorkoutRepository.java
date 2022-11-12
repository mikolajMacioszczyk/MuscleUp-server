package content.performedWorkout.repository;

import content.performedWorkout.entity.PerformedWorkout;

import java.util.UUID;

public interface PerformedWorkoutRepository {

    PerformedWorkout getById(UUID id);

    UUID save(PerformedWorkout performedWorkout);

    UUID update(PerformedWorkout performedWorkout);

    void delete(UUID id);
}
