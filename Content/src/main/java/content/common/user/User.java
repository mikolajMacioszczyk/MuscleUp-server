package content.common.user;

import java.util.UUID;

public record User(
        UUID userId,
        String name,
        String surname,
        String avatarUrl
) { }
