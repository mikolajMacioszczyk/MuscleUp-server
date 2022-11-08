package groups.group.repository;


import groups.group.entity.Group;
import groups.group.entity.GroupDto;

import java.util.List;
import java.util.Optional;
import java.util.UUID;

public interface GroupQuery {

    Group getById(UUID id);

    Optional<GroupDto> findGroupById(UUID id);

    List<GroupDto> getAllGroups();
}
