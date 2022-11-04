package groups.groupTrainer.trainer;

import java.util.UUID;

public record Trainer(
        UUID trainerId,
        String name,
        String surname,
        String avatarUrl
) { }
