package content.performedWorkout.entity;

import content.common.abstracts.AbstractEntity;
import content.workout.entity.Workout;
import org.hibernate.annotations.GenericGenerator;
import org.springframework.util.Assert;

import javax.persistence.*;
import java.time.ZonedDateTime;
import java.util.UUID;

@Entity
@Table(name = "performed_workout")
public class PerformedWorkout extends AbstractEntity {

    @Id
    @Column(name = "performed_workout_id")
    @GeneratedValue(generator = "UUID")
    @GenericGenerator(name = "UUID", strategy = "org.hibernate.id.UUIDGenerator")
    private UUID id;

    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "workout_id", nullable = false)
    private Workout workout;

    @Column(name = "user_id", nullable = false)
    private UUID userId;

    @Column(name = "time", nullable = false)
    private ZonedDateTime time;

    @Column(name = "entry_id")
    private UUID entryId;


    public PerformedWorkout(Workout workout, UUID userId, ZonedDateTime time, UUID entryId) {

        Assert.notNull(workout, "workout must not be null");
        Assert.notNull(userId, "userId must not be null");
        Assert.notNull(time, "time must not be null");
        Assert.notNull(entryId, "entryId must not be null");

        this.workout = workout;
        this. userId = userId;
        this.time = time;
        this.entryId = entryId;
    }

    @Override
    protected UUID getId() {
        return id;
    }

    public UUID getUserId() {
        return userId;
    }

    public ZonedDateTime getTime() {
        return time;
    }

    public UUID getEntryId() {
        return entryId;
    }
}
