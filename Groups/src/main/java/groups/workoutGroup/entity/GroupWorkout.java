package groups.workoutGroup.entity;

import groups.common.abstracts.AbstractEntity;
import groups.common.annotation.MustExist;
import groups.common.annotation.Reason;
import groups.common.annotation.UnknownForeignKey;
import groups.group.entity.Group;
import org.hibernate.annotations.GenericGenerator;
import org.springframework.util.Assert;

import javax.persistence.*;
import java.time.LocalDateTime;
import java.util.UUID;

@Entity
@Table(name = "class_workout")
public class GroupWorkout extends AbstractEntity {

    @Id
    @Column(name = "class_workout_id")
    @GeneratedValue(generator = "UUID")
    @GenericGenerator(name = "UUID", strategy = "org.hibernate.id.UUIDGenerator")
    private UUID id;

    @Column(name="start_time", nullable = false)
    private LocalDateTime startTime;

    @Column(name="end_time", nullable = false)
    private LocalDateTime endTime;

    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "class_id", nullable = false)
    private Group group;

    @UnknownForeignKey
    @Column(name = "workout_id", nullable = false)
    private UUID workoutId;


    @MustExist(reason = Reason.HIBERNATE)
    public GroupWorkout() {
    }


    public GroupWorkout(LocalDateTime startTime, LocalDateTime endTime, Group group, UUID workoutId) {

        Assert.notNull(startTime, "startTime must not be null");
        Assert.notNull(endTime, "endTime must not be null");
        Assert.notNull(group, "group must not be null");
        Assert.notNull(workoutId, "workoutId must not be null");

        this.startTime = startTime;
        this.endTime = endTime;
        this.group = group;
        this.workoutId = workoutId;
    }


    public void update(LocalDateTime startTime, LocalDateTime endTime, Group group, UUID workoutId) {

        Assert.notNull(startTime, "startTime must not be null");
        Assert.notNull(endTime, "endTime must not be null");
        Assert.notNull(group, "group must not be null");
        Assert.notNull(workoutId, "workoutId must not be null");

        this.startTime = startTime;
        this.endTime = endTime;
        this.group = group;
        this.workoutId = workoutId;
    }

    @Override
    public UUID getId() {
        return id;
    }

    public LocalDateTime getStartTime() {
        return startTime;
    }

    public LocalDateTime getEndTime() {
        return endTime;
    }

    public Group getGroup() {
        return group;
    }

    public UUID getWorkoutId() {
        return workoutId;
    }
}
