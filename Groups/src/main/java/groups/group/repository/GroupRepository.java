package groups.group.repository;

import groups.group.entity.Group;

public interface GroupRepository {

    Group getById(Long id);

    Long save(Group group);

    Long update(Group group);

    void delete(Long id);
}
