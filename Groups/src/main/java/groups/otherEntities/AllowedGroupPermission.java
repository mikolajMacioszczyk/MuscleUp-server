package groups.otherEntities;

import groups.common.abstracts.AbstractEntity;
import groups.common.annotation.MustExist;
import groups.common.annotation.Reason;
import groups.common.annotation.UnknownForeignKey;
import org.springframework.util.Assert;

import javax.persistence.*;

@Entity
@Table(name = "allowed_group_permission")
public class AllowedGroupPermission extends AbstractEntity {

    @Id
    private Long id;

    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "group_workout_id")
    private GroupWorkout groupWorkout;

    @UnknownForeignKey
    @Column(name = "permission_id", nullable = false, unique = true)
    private Long permissionId;


    @MustExist(reason = Reason.HIBERNATE)
    public AllowedGroupPermission() {
    }

    public AllowedGroupPermission(GroupWorkout groupWorkout, Long permissionId) {

        Assert.notNull(groupWorkout, "group must not be null");
        Assert.notNull(permissionId, "userId must not be null");

        this.groupWorkout = groupWorkout;
        this.permissionId = permissionId;
    }


    @Override
    public Long getId() {
        return id;
    }

    public GroupWorkout getGroupWorkout() {
        return groupWorkout;
    }

    public Long getPermissionId() {
        return permissionId;
    }
}
