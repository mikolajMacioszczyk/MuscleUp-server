package content.workoutExercise.repository;

import content.workoutExercise.entity.WorkoutExercise;

import java.util.UUID;

public interface WorkoutExerciseRepository {

    WorkoutExercise getById(UUID id);

    UUID save(WorkoutExercise workoutExercise);

    UUID update(WorkoutExercise workoutExercise);

    void delete(UUID id);
}
