package content.criterion.entity;

import content.common.abstracts.AbstractEntity;
import content.exercise.entity.Exercise;
import org.hibernate.annotations.GenericGenerator;
import org.springframework.util.Assert;

import javax.persistence.*;
import java.util.ArrayList;
import java.util.List;
import java.util.UUID;

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

    @ManyToMany(mappedBy = "criteria", fetch = LAZY)
    private final List<Exercise> exercises = new ArrayList<>();


    public Criterion(String name, String unit) {

        Assert.notNull(name, "name must not be null");

        this.name = name;
        this.unit = unit;
    }


    @Override
    protected UUID getId() {
        return id;
    }

    public String getName() {
        return name;
    }
}