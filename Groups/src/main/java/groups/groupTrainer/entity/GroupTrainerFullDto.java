package groups.groupTrainer.entity;

import java.util.UUID;

public record GroupTrainerFullDto(UUID id, UUID trainerId, UUID groupId) {
}
