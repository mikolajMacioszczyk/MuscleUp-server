package content.workout.repository;

import content.workout.entity.Workout;
import content.workout.entity.WorkoutDto;

import java.util.List;
import java.util.Optional;
import java.util.UUID;

public interface WorkoutQuery {

    Workout getById(UUID id);

    Optional<WorkoutDto> findById(UUID id);

    List<WorkoutDto> getAllWorkouts();

    List<UUID> getBodyPartsByWorkoutId(UUID id);
}
