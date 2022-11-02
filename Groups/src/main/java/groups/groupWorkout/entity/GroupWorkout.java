package groups.groupWorkout.entity;

import groups.common.abstracts.AbstractEntity;
import groups.common.annotation.MustExist;
import groups.common.annotation.Reason;
import groups.common.annotation.UnknownForeignKey;
import groups.group.entity.Group;
import org.hibernate.annotations.GenericGenerator;
import org.springframework.util.Assert;

import javax.persistence.*;
import java.util.UUID;

@Entity
@Table(name = "class_workout")
public class GroupWorkout extends AbstractEntity {

    @Id
    @Column(name = "class_workout_id")
    @GeneratedValue(generator = "UUID")
    @GenericGenerator(name = "UUID", strategy = "org.hibernate.id.UUIDGenerator")
    private UUID id;

    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "class_id", nullable = false)
    private Group group;

    @UnknownForeignKey
    @Column(name = "workout_id", nullable = false)
    private UUID workoutId;

    @Column(name = "location", nullable = false)
    private String location;

    @Column(name = "max_participants", nullable = false)
    private int maxParticipants;


    @MustExist(reason = Reason.HIBERNATE)
    public GroupWorkout() {
    }

    public GroupWorkout(Group group, UUID workoutId, String location, int maxParticipants) {

        Assert.notNull(group, "group must not be null");
        Assert.notNull(workoutId, "workoutId must not be null");
        Assert.notNull(location, "location must not be null");

        this.group = group;
        this.workoutId = workoutId;
        this.location = location;
        this.maxParticipants = maxParticipants;
    }


    public void update(Group group, UUID workoutId, String location, int maxParticipants) {

        Assert.notNull(group, "group must not be null");
        Assert.notNull(workoutId, "workoutId must not be null");
        Assert.notNull(location, "location must not be null");

        this.group = group;
        this.workoutId = workoutId;
        this.location = location;
        this.maxParticipants = maxParticipants;
    }

    @Override
    public UUID getId() {
        return id;
    }

    public Group getGroup() {
        return group;
    }

    public UUID getWorkoutId() {
        return workoutId;
    }

    public String getLocation() {
        return location;
    }

    public int getMaxParticipants() {
        return maxParticipants;
    }
}
