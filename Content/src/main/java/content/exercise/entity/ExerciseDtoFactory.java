package content.exercise.entity;

import content.criterion.entity.CriterionDtoFactory;

public class ExerciseDtoFactory {

    private final CriterionDtoFactory criterionDtoFactory;

    public ExerciseDtoFactory() {

        this.criterionDtoFactory = new CriterionDtoFactory();
    }


    public ExerciseDto create(Exercise exercise) {

        return new ExerciseDto(
                exercise.getId(),
                exercise.getFitnessClubId(),
                exercise.getName(),
                exercise.getDescription(),
                exercise.getImageUrl(),
                exercise.getCriteria()
                        .stream()
                        .map(criterionDtoFactory::create)
                        .toList()
        );
    }
}
