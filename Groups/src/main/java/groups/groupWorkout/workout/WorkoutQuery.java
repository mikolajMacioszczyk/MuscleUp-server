package groups.groupWorkout.workout;

import org.springframework.http.HttpStatus;

import java.util.UUID;

public interface WorkoutQuery {

    HttpStatus checkWorkoutId(UUID workoutId);
}
