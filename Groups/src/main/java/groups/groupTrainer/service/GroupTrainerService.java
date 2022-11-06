package groups.groupTrainer.service;

import groups.group.repository.GroupRepository;
import groups.groupTrainer.controller.form.GroupTrainerForm;
import groups.groupTrainer.entity.GroupTrainer;
import groups.groupTrainer.entity.GroupTrainerFactory;
import groups.groupTrainer.repository.GroupTrainerRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.util.Assert;

import java.util.UUID;

@Service
public class GroupTrainerService {

    private final GroupTrainerRepository groupTrainerRepository;
    private final GroupTrainerFactory groupTrainerFactory;
    private final GroupRepository groupRepository;


    @Autowired
    private GroupTrainerService(GroupTrainerRepository groupTrainerRepository,
                                GroupTrainerFactory groupTrainerFactory,
                                GroupRepository groupRepository) {

        Assert.notNull(groupTrainerRepository, "groupTrainerRepository must not be null");
        Assert.notNull(groupTrainerFactory, "groupTrainerFactory must not be null");
        Assert.notNull(groupRepository, "groupRepository must not be null");

        this.groupTrainerRepository = groupTrainerRepository;
        this.groupTrainerFactory = groupTrainerFactory;
        this.groupRepository = groupRepository;
    }


    public UUID assign(GroupTrainerForm groupTrainerForm) {

        Assert.notNull(groupTrainerForm, "groupTrainerForm must not be null");

        GroupTrainer groupTrainer = groupTrainerFactory.create(groupTrainerForm);

        return groupTrainerRepository.assign(groupTrainer);
    }

    public UUID update(UUID id, GroupTrainerForm groupTrainerForm) {

        Assert.notNull(id, "id must not be null");
        Assert.notNull(groupTrainerForm, "groupTrainerForm must not be null");

        GroupTrainer groupTrainer = groupTrainerRepository.getById(id);

        groupTrainer.update(
                groupRepository.getById(groupTrainerForm.groupId()),
                groupTrainerForm.trainerId()
        );

        return groupTrainerRepository.update(groupTrainer);
    }

    public void unassign(UUID groupTrainerId) {

        Assert.notNull(groupTrainerId, "groupTrainerId must not be null");

        groupTrainerRepository.unassign(groupTrainerId);
    }

    public void unassign(UUID trainerId, UUID groupId) {

        Assert.notNull(trainerId, "trainerId must not be null");
        Assert.notNull(groupId, "groupId must not be null");

        groupTrainerRepository.unassign(trainerId, groupId);
    }
}
