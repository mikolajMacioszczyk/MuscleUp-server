package content.criterion.entity;

public class CriterionDtoFactory {

    public CriterionDto create(Criterion criterion) {

        return new CriterionDto(
                criterion.getId(),
                criterion.getName(),
                criterion.getUnit()
        );
    }
}
