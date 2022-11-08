package groups.groupPermission.service;

import groups.groupPermission.controller.form.GroupPermissionForm;
import groups.groupPermission.entity.GroupPermission;
import groups.groupPermission.entity.GroupPermissionFactory;
import groups.groupPermission.repository.GroupPermissionRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.util.Assert;

import java.util.UUID;

@Service
public class GroupPermissionService {

    private final GroupPermissionRepository groupPermissionRepository;
    private final GroupPermissionFactory groupPermissionFactory;


    @Autowired
    private GroupPermissionService(GroupPermissionRepository groupPermissionRepository, GroupPermissionFactory groupPermissionFactory) {

        Assert.notNull(groupPermissionRepository, "groupPermissionRepository must not be null");
        Assert.notNull(groupPermissionFactory, "groupPermissionFactory must not be null");

        this.groupPermissionRepository = groupPermissionRepository;
        this.groupPermissionFactory = groupPermissionFactory;
    }


    public UUID assign(UUID groupId, UUID permissionId) {

        Assert.notNull(groupId, "groupId must not be null");
        Assert.notNull(permissionId, "permissionId must not be null");

        GroupPermission groupPermission = groupPermissionFactory.create(groupId, permissionId);

        return groupPermissionRepository.add(groupPermission);
    }

    public UUID assign(GroupPermissionForm form) {

        Assert.notNull(form, "form must not be null");

        return assign(form.groupId(), form.permissionId());
    }

    public void unassign(GroupPermissionForm form) {

        Assert.notNull(form, "form must not be null");

        groupPermissionRepository.remove(form.groupId(), form.permissionId());
    }
}
