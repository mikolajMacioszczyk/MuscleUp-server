package content.bodyPart.entity;

import content.common.abstracts.AbstractEntity;
import content.common.annotation.MustExist;
import content.workout.entity.Workout;
import org.hibernate.annotations.GenericGenerator;
import org.springframework.util.Assert;

import javax.persistence.*;
import java.util.ArrayList;
import java.util.List;
import java.util.UUID;

import static content.common.annotation.MustExist.Reason.HIBERNATE;
import static javax.persistence.FetchType.LAZY;

@Entity
@Table(name = "body_part")
public class BodyPart extends AbstractEntity {

    @Id
    @Column(name = "body_part_id")
    @GeneratedValue(generator = "UUID")
    @GenericGenerator(name = "UUID", strategy = "org.hibernate.id.UUIDGenerator")
    private UUID id;

    @Column(name = "name", nullable = false)
    private String name;

    @ManyToMany(mappedBy = "bodyParts", fetch = LAZY)
    private final List<Workout> workouts = new ArrayList<>();


    @MustExist(reason = HIBERNATE)
    public BodyPart() {
    }

    public BodyPart(String name) {

        Assert.notNull(name, "name must not be null");

        this.name = name;
    }


    public void update(String name) {

        Assert.notNull(name, "name must not be null");

        this.name = name;
    }

    @Override
    public UUID getId() {
        return id;
    }

    public String getName() {
        return name;
    }

    public void addWorkout(Workout workout) {

        workouts.add(workout);
    }

    public void removeWorkout(Workout workout) {

        workouts.remove(workout);
    }
}