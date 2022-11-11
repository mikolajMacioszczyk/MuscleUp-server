package content.criterion.repository;

import content.criterion.entity.CriterionDto;

import java.util.List;
import java.util.Optional;
import java.util.UUID;

public interface CriterionQuery {

    CriterionDto get(UUID id);

    Optional<CriterionDto> findById(UUID id);

    List<CriterionDto> getAllCriteria();

    List<CriterionDto> getAllActiveCriteria();
}
