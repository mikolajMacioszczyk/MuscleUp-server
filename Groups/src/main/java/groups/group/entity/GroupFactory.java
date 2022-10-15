package groups.group.entity;

import groups.group.controller.GroupFullForm;

public class GroupFactory {

    public Group create(GroupFullForm groupFullForm) {

        return new Group(
                groupFullForm.name(),
                groupFullForm.maxParticipants()
        );
    }
}
