package content.exercise.repository;

import content.exercise.entity.Exercise;
import content.exercise.entity.ExerciseDto;

import java.util.List;
import java.util.Optional;
import java.util.UUID;

public interface ExerciseQuery {

    Exercise getById(UUID id);

    Optional<ExerciseDto> findById(UUID id);

    List<ExerciseDto> getAllExercises();
}
