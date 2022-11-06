package groups.group.service;

import groups.group.controller.form.GroupForm;
import groups.group.entity.Group;
import groups.group.entity.GroupFactory;
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


    public UUID saveGroup(GroupForm groupForm) {

        Assert.notNull(groupForm, "groupForm must not be null");

        Group group = groupFactory.create(groupForm);

        return groupRepository.save(group);
    }

    public UUID updateGroup(UUID id, GroupForm groupForm) {

        Assert.notNull(id, "id must not be null");
        Assert.notNull(groupForm, "groupForm must not be null");

        Group group = groupRepository.getById(id);

        group.update(
                groupForm.name(),
                groupForm.description(),
                groupForm.repeatable()
        );

        return groupRepository.update(group);
    }

    public void deleteGroup(UUID id) {

        Assert.notNull(id, "id must not be null");

        groupRepository.delete(id);
    }
}

