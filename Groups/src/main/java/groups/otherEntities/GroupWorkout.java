package groups.otherEntities;

import groups.common.abstracts.AbstractEntity;
import groups.common.annotation.MustExist;
import groups.common.annotation.Reason;
import groups.common.annotation.UnknownForeignKey;
import groups.group.entity.Group;
import org.springframework.util.Assert;

import javax.persistence.*;
import java.time.LocalDateTime;

import static javax.persistence.GenerationType.AUTO;

@Entity
@Table(name = "group_workout")
public class GroupWorkout extends AbstractEntity {

    @Id
    @Column(name = "group_workout_id")
    @GeneratedValue(strategy = AUTO)
    private Long id;

    @Column(nullable = false)
    private LocalDateTime startTime;

    @Column(nullable = false)
    private LocalDateTime endTime;

    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "group_id")
    private Group group;

    @UnknownForeignKey
    @Column(name = "workout_id", nullable = false, unique = true)
    private Long workoutId;


    @MustExist(reason = Reason.HIBERNATE)
    public GroupWorkout() {
    }

    public GroupWorkout(LocalDateTime startTime, LocalDateTime endTime, Group group, Long workoutId) {

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
    public Long getId() {
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

    public Long getWorkoutId() {
        return workoutId;
    }
}
