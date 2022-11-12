package content.bodyPart.repository;

import content.bodyPart.entity.BodyPart;

import java.util.List;
import java.util.UUID;

public interface BodyPartRepository {

    BodyPart getById(UUID id);

    List<BodyPart> getByIds(List<UUID> ids);

    UUID save(BodyPart bodyPart);

    UUID update(BodyPart bodyPart);

    void delete(UUID id);
}
