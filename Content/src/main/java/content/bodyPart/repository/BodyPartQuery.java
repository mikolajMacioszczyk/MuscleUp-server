package content.bodyPart.repository;

import content.bodyPart.entity.BodyPart;
import content.bodyPart.entity.BodyPartDto;

import java.util.List;
import java.util.Optional;
import java.util.UUID;

public interface BodyPartQuery {

    BodyPart getById(UUID id);

    Optional<BodyPartDto> findById(UUID id);

    List<BodyPartDto> getAllBodyParts();
}
