package content.bodyPart.repository;

import content.bodyPart.entity.BodyPart;

import java.util.UUID;

public interface BodyPartRepository {

    BodyPart getById(UUID id);

    UUID save(BodyPart bodyPart);

    UUID update(BodyPart bodyPart);

    void delete(UUID id);
}
