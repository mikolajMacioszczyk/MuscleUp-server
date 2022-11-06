package groups.groupWorkout.entity;

import groups.common.abstracts.AbstractEntity;
import groups.common.annotation.MustExist;
import groups.common.annotation.UnknownForeignKey;
import groups.group.entity.Group;
import groups.workoutParticipant.entity.WorkoutParticipant;
import org.hibernate.annotations.GenericGenerator;
import org.springframework.lang.Nullable;
import org.springframework.util.Assert;

import javax.persistence.*;
import java.time.LocalDateTime;
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

    @Column(name = "location", nullable = false)
    private String location;

    @Column(name = "max_participants", nullable = false)
    private int maxParticipants;

    @Column(name="start_time", nullable = false)
    private LocalDateTime startTime;

    @Column(name="end_time", nullable = false)
    private LocalDateTime endTime;

    @Column(name="parent_id")
    private UUID parentId;

    @OneToMany(mappedBy = "groupWorkout", fetch = LAZY, cascade = ALL, orphanRemoval = true)
    private final List<WorkoutParticipant> workoutParticipants = new ArrayList<>();


    @MustExist(reason = HIBERNATE)
    public GroupWorkout() {
    }

    public GroupWorkout(Group group,
                        UUID workoutId,
                        String location,
                        int maxParticipants,
                        LocalDateTime startTime,
                        LocalDateTime endTime,
                        @Nullable UUID parentId) {

        Assert.notNull(group, "group must not be null");
        Assert.notNull(workoutId, "workoutId must not be null");
        Assert.notNull(location, "location must not be null");
        Assert.notNull(startTime, "startTime must not be null");
        Assert.notNull(endTime, "endTime must not be null");

        this.group = group;
        this.workoutId = workoutId;
        this.location = location;
        this.maxParticipants = maxParticipants;
        this.startTime = startTime;
        this.endTime = endTime;
        this.parentId = parentId;
    }


    public void update(Group group,
                       UUID workoutId,
                       String location,
                       int maxParticipants,
                       LocalDateTime startTime,
                       LocalDateTime endTime,
                       @Nullable UUID parentId) {

        Assert.notNull(group, "group must not be null");
        Assert.notNull(workoutId, "workoutId must not be null");
        Assert.notNull(location, "location must not be null");
        Assert.notNull(startTime, "startTime must not be null");
        Assert.notNull(endTime, "endTime must not be null");

        this.group = group;
        this.workoutId = workoutId;
        this.location = location;
        this.maxParticipants = maxParticipants;
        this.startTime = startTime;
        this.endTime = endTime;
        this.parentId = parentId;
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

    public LocalDateTime getStartTime() {
        return startTime;
    }

    public LocalDateTime getEndTime() {
        return endTime;
    }

    public UUID getParentId() {
        return parentId;
    }

    public List<WorkoutParticipant> getWorkoutParticipants() {
        return workoutParticipants;
    }
}
