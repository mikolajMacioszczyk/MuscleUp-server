package groups.groupWorkout.workout;

import org.springframework.http.HttpStatus;

import java.util.UUID;

public interface WorkoutValidator {

    HttpStatus checkWorkoutId(UUID workoutId);
}
