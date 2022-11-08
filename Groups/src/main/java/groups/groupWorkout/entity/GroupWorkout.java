package groups.groupWorkout.entity;

import groups.common.abstracts.AbstractEntity;
import groups.common.annotation.MustExist;
import groups.common.annotation.UnknownForeignKey;
import groups.group.entity.Group;
import groups.workoutParticipant.entity.WorkoutParticipant;
import org.hibernate.annotations.GenericGenerator;
import org.springframework.util.Assert;

import javax.persistence.*;
import java.time.ZonedDateTime;
import java.util.ArrayList;
import java.util.List;
import java.util.UUID;

import static groups.common.annotation.MustExist.Reason.HIBERNATE;
import static javax.persistence.CascadeType.ALL;
import static javax.persistence.FetchType.LAZY;

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

    @Column(name="start_time", nullable = false)
    private ZonedDateTime startTime;

    @Column(name="end_time", nullable = false)
    private ZonedDateTime endTime;

    @Column(name="clone_id")
    private UUID cloneId;

    @OneToMany(mappedBy = "groupWorkout", fetch = LAZY, cascade = ALL, orphanRemoval = true)
    private final List<WorkoutParticipant> workoutParticipants = new ArrayList<>();


    @MustExist(reason = HIBERNATE)
    public GroupWorkout() {
    }

    public GroupWorkout(Group group,
                        UUID workoutId,
                        ZonedDateTime startTime,
                        ZonedDateTime endTime,
                        UUID cloneId) {

        Assert.notNull(group, "group must not be null");
        Assert.notNull(workoutId, "workoutId must not be null");
        Assert.notNull(startTime, "startTime must not be null");
        Assert.notNull(endTime, "endTime must not be null");
        Assert.notNull(cloneId, "cloneId must not be null");

        this.group = group;
        this.workoutId = workoutId;
        this.startTime = startTime;
        this.endTime = endTime;
        this.cloneId = cloneId;
    }


    public void update(Group group,
                       UUID workoutId,
                       ZonedDateTime startTime,
                       ZonedDateTime endTime,
                       UUID cloneId) {

        Assert.notNull(group, "group must not be null");
        Assert.notNull(workoutId, "workoutId must not be null");
        Assert.notNull(startTime, "startTime must not be null");
        Assert.notNull(endTime, "endTime must not be null");
        Assert.notNull(cloneId, "cloneId must not be null");

        this.group = group;
        this.workoutId = workoutId;
        this.startTime = startTime;
        this.endTime = endTime;
        this.cloneId = cloneId;
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

    public ZonedDateTime getStartTime() {
        return startTime;
    }

    public ZonedDateTime getEndTime() {
        return endTime;
    }

    public UUID getCloneId() {
        return cloneId;
    }

    public List<WorkoutParticipant> getWorkoutParticipants() {
        return workoutParticipants;
    }
}
