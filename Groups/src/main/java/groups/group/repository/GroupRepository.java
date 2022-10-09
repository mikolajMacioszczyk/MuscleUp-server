package groups.group.repository;

import groups.group.entity.Group;

public interface GroupRepository {

    Group getById(Long id);

    Long saveOrUpdate(Group group);

    void delete(Long id);
}
