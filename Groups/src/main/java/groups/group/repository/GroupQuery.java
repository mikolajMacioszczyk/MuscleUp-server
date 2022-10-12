package groups.group.repository;


import groups.group.entity.Group;
import groups.group.entity.GroupFullDto;
import groups.group.entity.GroupNameDto;

import java.util.List;
import java.util.Optional;
import java.util.UUID;

public interface GroupQuery {

    Group getById(UUID id);

    Optional<GroupFullDto> findGroupById(UUID id);

    List<GroupFullDto> getAllGroups();

    List<GroupNameDto> getAllGroupNames();
}
