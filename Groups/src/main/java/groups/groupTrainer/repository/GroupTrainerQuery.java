package groups.groupTrainer.repository;

import groups.groupTrainer.entity.GroupTrainer;
import groups.groupTrainer.entity.GroupTrainerFullDto;

import java.util.List;
import java.util.Optional;
import java.util.UUID;

public interface GroupTrainerQuery {

    GroupTrainer getById(UUID id);

    List<GroupTrainerFullDto> getAllGroupsTrainers();

    Optional<GroupTrainerFullDto> findGroupTrainerById(UUID id);

    List<GroupTrainerFullDto> getAllGroupTrainerByGroupId(UUID groupId);

    List<GroupTrainerFullDto> getAllGroupTrainerByTrainerId(UUID trainerId);

    List<GroupTrainerFullDto> getAllGroupTrainerByGroupIdAndTrainerId(UUID groupId, UUID trainerId);
}
