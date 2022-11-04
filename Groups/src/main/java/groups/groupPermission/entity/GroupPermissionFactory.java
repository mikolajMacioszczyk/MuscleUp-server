package groups.groupPermission.entity;

import groups.group.repository.GroupQuery;
import groups.groupPermission.controller.form.GroupPermissionForm;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Component;
import org.springframework.util.Assert;

@Component
public class GroupPermissionFactory {

    private final GroupQuery groupQuery;


    @Autowired
    private GroupPermissionFactory(GroupQuery groupQuery) {

        Assert.notNull(groupQuery, "groupQuery must not be null");

        this.groupQuery = groupQuery;
    }


    public GroupPermission create(GroupPermissionForm groupPermissionForm) {

        Assert.notNull(groupPermissionForm, "groupPermissionForm must not be null");

        return new GroupPermission(
                groupQuery.getById(groupPermissionForm.groupId()),
                groupPermissionForm.permissionId()
        );
    }
}
