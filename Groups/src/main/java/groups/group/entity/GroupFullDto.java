package groups.group.entity;

import java.util.UUID;

public record GroupFullDto(UUID id, String name, Long maxParticipants) {

}
