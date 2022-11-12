package content.performedWorkout.repository;

import content.performedWorkout.entity.PerformedWorkout;
import content.performedWorkout.entity.PerformedWorkoutDto;

import java.util.List;
import java.util.UUID;

public interface PerformedWorkoutQuery {

    PerformedWorkout getById(UUID id);

    PerformedWorkoutDto get(UUID id);

    List<PerformedWorkoutDto> getAllPerformedWorkouts();

    List<PerformedWorkoutDto> getAllPerformedWorkoutsByUserId(UUID userId);

    List<PerformedWorkoutDto> getAllPerformedWorkoutsByCreatorId(UUID creatorId);

    List<PerformedWorkoutDto> getAllPerformedWorkoutsByWorkoutId(UUID workoutId);

    Integer getPerformancesByWorkoutId(UUID workoutId);
}
