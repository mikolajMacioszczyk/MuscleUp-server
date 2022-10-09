package groups.group.entity;

import org.springframework.util.Assert;

public class GroupNameDtoFactory {

    public GroupNameDto create(Group group) {

        Assert.notNull(group, "group must not be null");

        return new GroupNameDto(group.getId(), group.getName());
    }
}
