package content.criterion.repository;

import content.criterion.entity.Criterion;

import java.util.List;
import java.util.UUID;

public interface CriterionRepository {

    Criterion getById(UUID id);

    List<Criterion> getByIds(List<UUID> ids);

    UUID save(Criterion criterion);

    UUID update(Criterion criterion);
}
