package groups.otherEntities;

import groups.common.abstracts.AbstractEntity;
import groups.common.annotation.MustExist;
import groups.common.annotation.Reason;
import groups.common.annotation.UnknownForeignKey;
import org.springframework.util.Assert;

import javax.persistence.*;
import java.util.UUID;

@Entity
@Table(name = "allowed_class_permission")
public class AllowedGroupPermission extends AbstractEntity {

    @Id
    private UUID id;

    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "class_workout_id")
    private GroupWorkout groupWorkout;

    @UnknownForeignKey
    @Column(name = "permission_id", nullable = false)
    private UUID permissionId;


    @MustExist(reason = Reason.HIBERNATE)
    public AllowedGroupPermission() {
    }

    public AllowedGroupPermission(GroupWorkout groupWorkout, UUID permissionId) {

        Assert.notNull(groupWorkout, "groupWorkout must not be null");
        Assert.notNull(permissionId, "permissionId must not be null");

        this.groupWorkout = groupWorkout;
        this.permissionId = permissionId;
    }


    @Override
    public UUID getId() {
        return id;
    }

    public GroupWorkout getGroupWorkout() {
        return groupWorkout;
    }

    public UUID getPermissionId() {
        return permissionId;
    }
}
