package groups.group.entity;

import groups.group.controller.form.GroupForm;
import org.springframework.util.Assert;

public class GroupFactory {

    public Group create(GroupForm groupForm) {

        Assert.notNull(groupForm, "groupFullForm must not be null");

        return new Group(
                groupForm.name(),
                groupForm.description(),
                groupForm.repeatable()
        );
    }
}
