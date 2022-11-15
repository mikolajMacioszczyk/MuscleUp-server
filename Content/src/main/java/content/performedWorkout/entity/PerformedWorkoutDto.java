package content.performedWorkout.entity;

import content.workout.entity.WorkoutDto;

import java.time.ZonedDateTime;
import java.util.UUID;

public record PerformedWorkoutDto(
        WorkoutDto workoutDto,
        UUID userId,
        ZonedDateTime time
) { }
