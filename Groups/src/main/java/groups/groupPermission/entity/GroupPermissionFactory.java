package groups.groupPermission.entity;

import groups.group.repository.GroupQuery;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Component;
import org.springframework.util.Assert;

import java.util.UUID;

@Component
public class GroupPermissionFactory {

    private final GroupQuery groupQuery;


    @Autowired
    private GroupPermissionFactory(GroupQuery groupQuery) {

        Assert.notNull(groupQuery, "groupQuery must not be null");

        this.groupQuery = groupQuery;
    }


    public GroupPermission create(UUID groupId, UUID permissionId) {

        Assert.notNull(groupId, "groupId must not be null");
        Assert.notNull(permissionId, "permissionId must not be null");

        return new GroupPermission(
                groupQuery.getById(groupId),
                permissionId
        );
    }
}
