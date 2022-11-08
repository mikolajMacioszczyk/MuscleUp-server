package groups.workoutParticipant.entity;

import groups.common.abstracts.AbstractEntity;
import groups.common.annotation.MustExist;
import groups.common.annotation.UnknownForeignKey;
import groups.groupWorkout.entity.GroupWorkout;
import org.hibernate.annotations.GenericGenerator;
import org.springframework.util.Assert;

import javax.persistence.*;
import java.util.UUID;

import static groups.common.annotation.MustExist.Reason.HIBERNATE;

@Entity
@Table(name = "class_workout_participant")
public class WorkoutParticipant extends AbstractEntity {

    @Id
    @Column(name = "class_workout_participant_id")
    @GeneratedValue(generator = "UUID")
    @GenericGenerator(name = "UUID", strategy = "org.hibernate.id.UUIDGenerator")
    private UUID id;

    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "class_workout_id", nullable = false)
    private GroupWorkout groupWorkout;

    @UnknownForeignKey
    @Column(name = "user_id", nullable = false)
    private UUID userId;


    @MustExist(reason = HIBERNATE)
    public WorkoutParticipant() {
    }

    public WorkoutParticipant(GroupWorkout groupWorkout, UUID userId) {

        Assert.notNull(groupWorkout, "groupWorkout must not be null");
        Assert.notNull(userId, "userId must not be null");

        this.groupWorkout = groupWorkout;
        this.userId = userId;
    }


    @Override
    public UUID getId() {
        return id;
    }

    public GroupWorkout getGroupWorkout() {
        return groupWorkout;
    }

    public UUID getUserId() {
        return userId;
    }
}