package groups.groupTrainer.service;

import groups.groupTrainer.controller.GroupTrainerForm;
import groups.groupTrainer.entity.GroupTrainer;
import groups.groupTrainer.entity.GroupTrainerFactory;
import groups.groupTrainer.repository.GroupTrainerRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.util.Assert;

import java.util.UUID;

public class GroupTrainerService {

    private final GroupTrainerRepository groupTrainerRepository;
    private final GroupTrainerFactory groupTrainerFactory;


    @Autowired
    private GroupTrainerService(GroupTrainerRepository groupTrainerRepository, GroupTrainerFactory groupTrainerFactory) {

        Assert.notNull(groupTrainerRepository, "groupTrainerRepository must not be null");
        Assert.notNull(groupTrainerFactory, "groupTrainerFactory must not be null");

        this.groupTrainerRepository = groupTrainerRepository;
        this.groupTrainerFactory = groupTrainerFactory;
    }


    public UUID assign(GroupTrainerForm groupTrainerForm) {

        Assert.notNull(groupTrainerForm, "groupTrainerForm must not be null");

        GroupTrainer groupTrainer = groupTrainerFactory.create(groupTrainerForm);

        return groupTrainerRepository.assign(groupTrainer);
    }

    public void unassign(UUID groupTrainerId) {

        Assert.notNull(groupTrainerId, "groupTrainerId must not be null");

        groupTrainerRepository.unassign(groupTrainerId);
    }

    public void unassign(GroupTrainerForm groupTrainerForm) {

        Assert.notNull(groupTrainerForm, "groupTrainerForm must not be null");

        groupTrainerRepository.unassign(groupTrainerForm.trainerId(), groupTrainerForm.groupId());
    }
}
