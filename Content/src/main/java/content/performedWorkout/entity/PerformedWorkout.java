package content.performedWorkout.entity;

import content.common.abstracts.AbstractEntity;
import content.common.annotation.MustExist;
import content.workout.entity.Workout;
import org.hibernate.annotations.GenericGenerator;
import org.springframework.util.Assert;

import javax.persistence.*;
import java.time.ZonedDateTime;
import java.util.UUID;

import static content.common.annotation.MustExist.Reason.HIBERNATE;

@Entity
@Table(name = "performed_workout")
public class PerformedWorkout extends AbstractEntity {

    @Id
    @Column(name = "performed_workout_id")
    @GeneratedValue(generator = "UUID")
    @GenericGenerator(name = "UUID", strategy = "org.hibernate.id.UUIDGenerator")
    private UUID id;

    @ManyToOne(fetch = FetchType.EAGER)
    @JoinColumn(name = "workout_id", nullable = false)
    private Workout workout;

    @Column(name = "user_id", nullable = false)
    private UUID userId;

    @Column(name = "time", nullable = false)
    private ZonedDateTime time;


    @MustExist(reason = HIBERNATE)
    public PerformedWorkout() {
    }

    public PerformedWorkout(Workout workout, UUID userId, ZonedDateTime time) {

        Assert.notNull(workout, "workout must not be null");
        Assert.notNull(userId, "userId must not be null");
        Assert.notNull(time, "time must not be null");

        this.workout = workout;
        this. userId = userId;
        this.time = time;
    }

    @Override
    protected UUID getId() {
        return id;
    }

    public Workout getWorkout() {
        return workout;
    }

    public UUID getUserId() {
        return userId;
    }

    public ZonedDateTime getTime() {
        return time;
    }
}
