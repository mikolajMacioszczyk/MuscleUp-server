package groups.group.service;

import groups.group.controller.GroupFullForm;
import groups.group.entity.Group;
import groups.group.entity.GroupFactory;
import groups.group.entity.GroupFullDto;
import groups.group.repository.GroupRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.util.Assert;

import java.util.UUID;

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


    public UUID updateGroup(GroupFullDto groupFullDto) {

        Assert.notNull(groupFullDto, "groupFullDto must not be null");

        Group group = groupRepository.getById(groupFullDto.id());

        group.update(
                groupFullDto.name(),
                groupFullDto.maxParticipants()
        );

        return groupRepository.update(group);
    }

    public UUID saveGroup(GroupFullForm groupFullForm) {

        Assert.notNull(groupFullForm, "groupForm must not be null");

        Group group = groupFactory.create(groupFullForm);

        return groupRepository.save(group);
    }

    public void deleteGroup(UUID idToRemove) {

        Assert.notNull(idToRemove, "idToRemove must not be null");

        groupRepository.delete(idToRemove);
    }
}

