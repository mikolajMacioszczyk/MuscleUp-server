package content.workout.repository;

import content.workout.entity.Workout;
import content.workout.entity.WorkoutComparisonDto;
import content.workout.entity.WorkoutDto;

import java.util.List;
import java.util.Optional;
import java.util.UUID;

public interface WorkoutQuery {

    Workout getById(UUID id);

    WorkoutComparisonDto getForComparison(UUID id);

    Optional<WorkoutDto> findById(UUID id, UUID fitnessClubId);

    List<WorkoutDto> getAllWorkouts(UUID fitnessClubId);

    List<WorkoutDto> getAllActiveWorkouts(UUID fitnessClubId);
}
