package groups.groupTrainer.entity;

import org.springframework.util.Assert;

public class GroupTrainerFullDtoFactory {

    public GroupTrainerFullDto create(GroupTrainer groupTrainer) {

        Assert.notNull(groupTrainer, "groupTrainer must not be null");

        return new GroupTrainerFullDto(
                groupTrainer.getId(),
                groupTrainer.getTrainerId(),
                groupTrainer.getGroup().getId()
        );
    }
}
