package groups.workoutPermission.entity;

import groups.common.abstracts.AbstractEntity;
import groups.common.annotation.MustExist;
import groups.common.annotation.Reason;
import groups.common.annotation.UnknownForeignKey;
import groups.workout.entity.GroupWorkout;
import org.springframework.util.Assert;

import javax.persistence.*;
import java.util.UUID;

@Entity
@Table(name = "class_permission_allowed")
public class GroupPermissionAllowed extends AbstractEntity {

    @Id
    private UUID id;

    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "class_workout_id")
    private GroupWorkout groupWorkout;

    @UnknownForeignKey
    @Column(name = "permission_id", nullable = false)
    private UUID permissionId;


    @MustExist(reason = Reason.HIBERNATE)
    public GroupPermissionAllowed() {
    }

    public GroupPermissionAllowed(GroupWorkout groupWorkout, UUID permissionId) {

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
