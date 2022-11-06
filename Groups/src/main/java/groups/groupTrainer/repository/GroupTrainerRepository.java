package groups.groupTrainer.repository;

import groups.groupTrainer.entity.GroupTrainer;

import java.util.UUID;

public interface GroupTrainerRepository {

    GroupTrainer getById(UUID id);

    UUID assign(GroupTrainer groupTrainer);

    UUID update(GroupTrainer groupTrainer);

    void unassign(UUID groupTrainerId);

    void unassign(UUID trainerId, UUID groupId);
}
