package groups.group.repository;


import groups.group.entity.Group;
import groups.group.entity.GroupFullDto;
import groups.group.entity.GroupNameDto;

import java.util.List;

public interface GroupQuery {

    Group getById(Long id);

    List<GroupFullDto> getAllGroups();

    List<GroupNameDto> getAllGroupNames();
}
