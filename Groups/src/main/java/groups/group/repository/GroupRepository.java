package groups.group.repository;

import groups.group.entity.Group;

import java.util.UUID;

public interface GroupRepository {

    Group getById(UUID id);

    UUID save(Group group);

    UUID update(Group group);

    void delete(UUID id);
}
