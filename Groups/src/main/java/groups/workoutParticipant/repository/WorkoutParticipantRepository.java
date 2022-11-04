package groups.workoutParticipant.repository;

import groups.workoutParticipant.entity.WorkoutParticipant;

import java.util.UUID;

public interface WorkoutParticipantRepository {

    WorkoutParticipant getById(UUID id);

    UUID assign(WorkoutParticipant workoutParticipant);

    void unassign(UUID workoutParticipantId);

    void unassign(UUID groupWorkoutId, UUID gympassId);
}
