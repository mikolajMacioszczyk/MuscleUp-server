package groups.groupWorkout.repository;

import groups.common.wrappers.TimeWrapper;
import groups.groupWorkout.entity.GroupWorkout;
import groups.groupWorkout.entity.GroupWorkoutDto;

import java.util.List;
import java.util.Optional;
import java.util.UUID;

public interface GroupWorkoutQuery {

    GroupWorkout getById(UUID id);

    TimeWrapper getTimeById(UUID id);

    Optional<GroupWorkoutDto> findGroupWorkoutById(UUID id);

    List<GroupWorkoutDto> getAllGroupsWorkouts();

    UUID getCloneIdById(UUID id);

    List<UUID> getFutureGroupWorkoutsByCloneId(UUID id);

    List<GroupWorkoutDto> getAllRepeatableGroupWorkoutsDayAhead();

    UUID getFitnessClubIdByGroupWorkoutId(UUID id);
}
