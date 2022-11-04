package groups.schedule.dto;

import groups.group.entity.Group;
import org.springframework.util.Assert;

public class ScheduleGroupDtoFactory {

    public ScheduleGroupDto create(Group group) {

        Assert.notNull(group, "group must not be null");

        return new ScheduleGroupDto(
                group.getId(),
                group.getName(),
                group.getDescription(),
                group.getStartTime(),
                group.getEndTime(),
                group.isRepeatable()
        );
    }
}
