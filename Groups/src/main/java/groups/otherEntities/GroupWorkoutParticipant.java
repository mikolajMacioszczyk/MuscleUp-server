package groups.otherEntities;

import groups.common.abstracts.AbstractEntity;
import groups.common.annotation.MustExist;
import groups.common.annotation.Reason;
import groups.common.annotation.UnknownForeignKey;
import org.springframework.util.Assert;

import javax.persistence.*;
import java.util.UUID;

@Entity
@Table(name = "group_workout_participant")
public class GroupWorkoutParticipant extends AbstractEntity {

    @Id
    private UUID id;

    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "group_workout_id")
    private GroupWorkout groupWorkout;

    @UnknownForeignKey
    @Column(name = "gympass_id", nullable = false, unique = true)
    private UUID gympassId;


    @MustExist(reason = Reason.HIBERNATE)
    public GroupWorkoutParticipant() {
    }

    public GroupWorkoutParticipant(GroupWorkout groupWorkout, UUID gympassId) {

        Assert.notNull(groupWorkout, "group must not be null");
        Assert.notNull(gympassId, "userId must not be null");

        this.groupWorkout = groupWorkout;
        this.gympassId = gympassId;
    }


    @Override
    public UUID getId() {
        return id;
    }

    public GroupWorkout getGroupWorkout() {
        return groupWorkout;
    }

    public UUID getGympassId() {
        return gympassId;
    }
}