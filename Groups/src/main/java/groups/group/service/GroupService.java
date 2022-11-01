package groups.group.service;

import groups.group.controller.form.GroupFullForm;
import groups.group.entity.Group;
import groups.group.entity.GroupFactory;
import groups.group.entity.GroupFullDto;
import groups.group.repository.GroupRepository;
import groups.groupTrainer.service.GroupTrainerService;
import groups.workoutGroup.service.GroupWorkoutService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.util.Assert;

import java.util.UUID;

@Service
public class GroupService {

    private final GroupRepository groupRepository;
    private final GroupFactory groupFactory;
    private final GroupTrainerService groupTrainerService;
    private final GroupWorkoutService groupWorkoutService;


    @Autowired
    private GroupService(GroupRepository groupRepository,
                         GroupTrainerService groupTrainerService,
                         GroupWorkoutService groupWorkoutService) {

        Assert.notNull(groupRepository, "groupRepository must not be null");
        Assert.notNull(groupTrainerService, "groupTrainerService must not be null");
        Assert.notNull(groupWorkoutService, "groupWorkoutService must not be null");

        this.groupRepository = groupRepository;
        this.groupTrainerService = groupTrainerService;
        this.groupWorkoutService = groupWorkoutService;
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

        groupTrainerService.unassignAllByGroupId(idToRemove);
        groupWorkoutService.deleteAllByGroupId(idToRemove);
        groupRepository.delete(idToRemove);
    }
}

