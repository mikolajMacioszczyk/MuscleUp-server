package content.workoutExercise.repository;

import content.workoutExercise.entity.WorkoutExercise;

import java.util.Optional;
import java.util.UUID;

public interface WorkoutExerciseQuery {

    WorkoutExercise getById(UUID id);

    Optional<WorkoutExercise> findById(UUID id);

    boolean isExerciseConnected(UUID exerciseId);
}
