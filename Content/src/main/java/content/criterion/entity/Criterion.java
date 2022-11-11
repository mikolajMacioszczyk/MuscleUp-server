package content.criterion.entity;

import content.common.abstracts.AbstractEntity;
import content.common.annotation.MustExist;
import content.exercise.entity.Exercise;
import org.hibernate.annotations.GenericGenerator;
import org.springframework.util.Assert;

import javax.persistence.*;
import java.util.ArrayList;
import java.util.List;
import java.util.UUID;

import static content.common.annotation.MustExist.Reason.HIBERNATE;
import static javax.persistence.FetchType.LAZY;

@Entity
@Table(name = "criterion")
public class Criterion extends AbstractEntity {

    @Id
    @Column(name = "criterion_id")
    @GeneratedValue(generator = "UUID")
    @GenericGenerator(name = "UUID", strategy = "org.hibernate.id.UUIDGenerator")
    private UUID id;

    @Column(name = "name", nullable = false)
    private String name;

    @Column(name = "unit")
    private String unit;

    @Column(name = "active")
    private boolean active;

    @ManyToMany(mappedBy = "criteria", fetch = LAZY)
    private final List<Exercise> exercises = new ArrayList<>();


    @MustExist(reason = HIBERNATE)
    public Criterion() {
    }

    public Criterion(String name, String unit, boolean active) {

        Assert.notNull(name, "name must not be null");

        this.name = name;
        this.unit = unit;
        this.active = active;
    }


    @Override
    public UUID getId() {
        return id;
    }

    public String getName() {
        return name;
    }

    public String getUnit() {
        return unit;
    }

    public boolean isActive() {
        return active;
    }

    public void activate() {
        this.active = true;
    }

    public void deactivate() {
        this.active = false;
    }
}