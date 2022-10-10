package groups.group.entity;

import groups.group.controller.GroupForm;

public class GroupFactory {

    public Group create(GroupForm groupForm) {

        return new Group(
                null,
                groupForm.name(),
                groupForm.maxParticipants()
        );
    }
}
