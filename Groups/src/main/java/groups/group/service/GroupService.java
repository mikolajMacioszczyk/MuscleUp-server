package groups.group.service;

import groups.group.entity.Group;
import groups.group.entity.GroupFactory;
import groups.group.entity.GroupFullDto;
import groups.group.repository.GroupRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.util.Assert;

@Service
public class GroupService {

    private final GroupRepository groupRepository;
    private final GroupFactory groupFactory;


    @Autowired
    private GroupService(GroupRepository groupRepository) {

        Assert.notNull(groupRepository, "groupRepository must not be null");

        this.groupRepository = groupRepository;
        this.groupFactory = new GroupFactory();
    }


    public Long updateGroup(GroupFullDto groupFullDto) {

        Assert.notNull(groupFullDto, "groupFullDto must not be null");

        Group group = groupRepository.getById(groupFullDto.getId());

        group.update(
                groupFullDto.getName(),
                groupFullDto.getMaxParticipants()
        );

        return groupRepository.update(group);
    }

    public Long saveGroup(GroupFullDto groupFullDto) {

        Assert.notNull(groupFullDto, "groupFullDto must not be null");

        Group group = groupFactory.create(groupFullDto);

        return groupRepository.save(group);
    }

    public void deleteGroup(Long idToRemove) {

        Assert.notNull(idToRemove, "idToRemove must not be null");

        groupRepository.delete(idToRemove);
    }
}

