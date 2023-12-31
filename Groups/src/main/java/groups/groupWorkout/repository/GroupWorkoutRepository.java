package groups.groupWorkout.repository;

import groups.groupWorkout.entity.GroupWorkout;

import java.util.UUID;

public interface GroupWorkoutRepository {

    GroupWorkout getById(UUID id);

    UUID save(GroupWorkout groupWorkout);

    UUID update(GroupWorkout groupWorkout);

    void delete(UUID id);
}
