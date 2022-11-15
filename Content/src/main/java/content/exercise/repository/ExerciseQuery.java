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

    Optional<ExerciseDto> findById(UUID id, UUID fitnessClubId);

    List<ExerciseDto> getAllExercises(UUID fitnessClubId);

    List<ExerciseDto> getAllActiveExercises(UUID fitnessClubId);

    List<UUID> getAllAppliedCriteriaById(UUID id);
}
