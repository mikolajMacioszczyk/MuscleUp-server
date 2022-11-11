package content.exercise.repository;

import content.exercise.entity.Exercise;
import content.exercise.entity.ExerciseDto;

import java.util.List;
import java.util.Optional;
import java.util.UUID;

public interface ExerciseQuery {

    // TODO usunąć encję z query, bo nie jest wykorzystywana i zawsze zwracać DTO jak poniżej
    Exercise getById(UUID id);

    ExerciseDto get(UUID id);

    Optional<ExerciseDto> findById(UUID id);

    List<ExerciseDto> getAllExercises();

    List<ExerciseDto> getAllActiveExercises();
}
