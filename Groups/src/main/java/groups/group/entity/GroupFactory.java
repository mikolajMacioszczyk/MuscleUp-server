package groups.group.entity;

import groups.group.controller.GroupFullForm;
import org.springframework.util.Assert;

public class GroupFactory {

    public Group create(GroupFullForm groupFullForm) {

        Assert.notNull(groupFullForm, "groupFullForm must not be null");

        return new Group(
                groupFullForm.name(),
                groupFullForm.maxParticipants()
        );
    }
}
