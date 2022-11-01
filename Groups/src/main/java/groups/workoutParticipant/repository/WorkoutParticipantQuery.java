package groups.workoutParticipant.repository;

import groups.workoutParticipant.entity.WorkoutParticipant;
import groups.workoutParticipant.entity.WorkoutParticipantFullDto;

import java.util.List;
import java.util.Optional;
import java.util.UUID;

public interface WorkoutParticipantQuery {

    WorkoutParticipant getById(UUID id);

    List<WorkoutParticipantFullDto> getAllWorkoutParticipants();

    Optional<WorkoutParticipantFullDto> findWorkoutParticipantById(UUID id);

    List<WorkoutParticipantFullDto> getAllWorkoutParticipantsByGroupWorkoutId(UUID groupWorkoutId);

    List<WorkoutParticipantFullDto> getAllWorkoutParticipantsByGympassId(UUID gympassId);

    List<WorkoutParticipantFullDto> getAllWorkoutParticipantsByGroupWorkoutIdAndGympassId(UUID groupWorkoutId, UUID gympassId);
}
