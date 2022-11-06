package groups.group.entity;

import org.springframework.util.Assert;

public class GroupFullDtoFactory {

    public GroupFullDto create(Group group) {

        Assert.notNull(group, "group must not be null");

        return new GroupFullDto(
                group.getId(),
                group.getName(),
                group.getDescription(),
                group.isRepeatable()
        );
    }
}
