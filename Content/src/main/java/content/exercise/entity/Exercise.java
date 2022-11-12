package content.exercise.entity;

import content.common.abstracts.AbstractEntity;
import content.common.annotation.MustExist;
import content.criterion.entity.Criterion;
import content.workoutExercise.entity.WorkoutExercise;
import org.hibernate.annotations.GenericGenerator;
import org.springframework.lang.Nullable;
import org.springframework.util.Assert;

import javax.persistence.*;
import java.util.ArrayList;
import java.util.List;
import java.util.UUID;

import static content.common.annotation.MustExist.Reason.HIBERNATE;
import static javax.persistence.CascadeType.ALL;
import static javax.persistence.FetchType.LAZY;

@Entity
@Table(name = "exercise")
public class Exercise extends AbstractEntity {

    @Id
    @Column(name = "exercise_id")
    @GeneratedValue(generator = "UUID")
    @GenericGenerator(name = "UUID", strategy = "org.hibernate.id.UUIDGenerator")
    private UUID id;

    @Column(name = "name", nullable = false)
    private String name;

    @Column(name = "description", nullable = false)
    private String description;

    @Column(name = "image_url", nullable = false)
    private String imageUrl;

    @Column(name = "video_url")
    private String videoUrl;

    @Column(name = "latest")
    private boolean latest;

    @OneToMany(mappedBy = "exercise", fetch = LAZY, cascade = ALL, orphanRemoval = true)
    private final List<WorkoutExercise> workoutExercises = new ArrayList<>();

    @ManyToMany(fetch = LAZY)
    @JoinTable(
            name = "exercise_criterion",
            joinColumns = @JoinColumn(name = "exercise_id"),
            inverseJoinColumns = @JoinColumn(name = "criterion_id")
    )
    private List<Criterion> criteria = new ArrayList<>();


    @MustExist(reason = HIBERNATE)
    public Exercise() {
    }

    public Exercise(
            String name,
            String description,
            @Nullable String imageUrl,
            @Nullable String videoUrl,
            boolean latest,
            List<Criterion> criteria) {

        Assert.notNull(name, "name must not be null");
        Assert.notNull(description, "description must not be null");
        Assert.notNull(criteria, "criteria must not be null");
        criteria.forEach( criterion -> Assert.notNull(criterion, "criterion must not be null"));

        this.name = name;
        this.description = description;
        this.imageUrl = imageUrl;
        this.videoUrl = videoUrl;
        this.latest = latest;
        this.criteria = criteria;
    }


    @Override
    public UUID getId() {
        return id;
    }

    public String getName() {
        return name;
    }

    public String getDescription() {
        return description;
    }

    public String getImageUrl() {
        return imageUrl;
    }

    public String getVideoUrl() {
        return videoUrl;
    }

    public boolean isLatest() {
        return latest;
    }

    public void setLatest(boolean latest) {
        this.latest = latest;
    }

    public List<Criterion> getCriteria() {
        return criteria;
    }

    public List<WorkoutExercise> getWorkoutExercises() {
        return workoutExercises;
    }
}