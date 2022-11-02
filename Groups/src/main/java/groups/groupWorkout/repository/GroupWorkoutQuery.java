package groups.groupWorkout.repository;

import groups.groupWorkout.entity.GroupWorkout;
import groups.groupWorkout.entity.GroupWorkoutFullDto;

import java.util.List;
import java.util.Optional;
import java.util.UUID;

public interface GroupWorkoutQuery {

    GroupWorkout getById(UUID id);

    Optional<GroupWorkoutFullDto> findGroupWorkoutById(UUID id);

    List<GroupWorkoutFullDto> getAllGroupWorkoutByGroupId(UUID groupId);

    List<GroupWorkoutFullDto> getAllGroupWorkoutByWorkoutId(UUID workoutId);

    List<GroupWorkoutFullDto> getAllGroupsWorkouts();
}
