package groups.groupPermission.entity;

import groups.common.abstracts.AbstractEntity;
import groups.common.annotation.MustExist;
import groups.common.annotation.UnknownForeignKey;
import groups.group.entity.Group;
import org.hibernate.annotations.GenericGenerator;
import org.springframework.util.Assert;

import javax.persistence.*;
import java.util.UUID;

import static groups.common.annotation.MustExist.Reason.HIBERNATE;

@Entity
@Table(name = "class_permission")
public class GroupPermission extends AbstractEntity {

    @Id
    @Column(name = "class_permission_id")
    @GeneratedValue(generator = "UUID")
    @GenericGenerator(name = "UUID", strategy = "org.hibernate.id.UUIDGenerator")
    private UUID id;

    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "class_id")
    private Group group;

    @UnknownForeignKey
    @Column(name = "permission_id", nullable = false)
    private UUID permissionId;


    @MustExist(reason = HIBERNATE)
    public GroupPermission() {
    }

    public GroupPermission(Group group, UUID permissionId) {

        Assert.notNull(group, "group must not be null");
        Assert.notNull(permissionId, "permissionId must not be null");

        this.group = group;
        this.permissionId = permissionId;
    }


    @Override
    public UUID getId() {
        return id;
    }

    public Group getGroup() {
        return group;
    }

    public UUID getPermissionId() {
        return permissionId;
    }
}
