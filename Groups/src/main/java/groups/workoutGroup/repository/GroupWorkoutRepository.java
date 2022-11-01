package groups.workoutGroup.repository;

import groups.common.wrappers.UuidWrapper;
import groups.workoutGroup.entity.GroupWorkout;

import java.util.List;
import java.util.UUID;

public interface GroupWorkoutRepository {

    GroupWorkout getById(UUID id);

    List<UuidWrapper> getIdsByGroupId(UUID id);

    UUID save(GroupWorkout groupWorkout);

    UUID update(GroupWorkout groupWorkout);

    void delete(UUID id);
}
