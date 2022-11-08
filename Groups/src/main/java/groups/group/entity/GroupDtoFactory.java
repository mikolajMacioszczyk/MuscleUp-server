package groups.group.entity;

import org.springframework.util.Assert;

public class GroupDtoFactory {

    public GroupDto create(Group group) {

        Assert.notNull(group, "group must not be null");

        return new GroupDto(
                group.getId(),
                group.getName(),
                group.getTrainerId(),
                group.getFitnessClubId(),
                group.getDescription(),
                group.getLocation(),
                group.getMaxParticipants(),
                group.isRepeatable()
        );
    }
}
