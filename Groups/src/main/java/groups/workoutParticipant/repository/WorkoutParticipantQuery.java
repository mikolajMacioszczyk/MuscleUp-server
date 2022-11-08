package groups.workoutParticipant.repository;

import groups.workoutParticipant.entity.WorkoutParticipant;
import groups.workoutParticipant.entity.WorkoutParticipantDto;

import java.util.List;
import java.util.UUID;

public interface WorkoutParticipantQuery {

    WorkoutParticipant getById(UUID id);

    List<WorkoutParticipantDto> getAllWorkoutParticipantsByGroupWorkoutIdAndUserId(UUID groupWorkoutId, UUID userId);
}
