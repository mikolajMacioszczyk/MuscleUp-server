package content.workout.entity;

import content.bodyPart.entity.BodyPart;
import content.common.abstracts.AbstractEntity;
import content.common.annotation.MustExist;
import content.workoutExercise.entity.WorkoutExercise;
import org.hibernate.annotations.GenericGenerator;
import org.springframework.util.Assert;

import javax.persistence.*;
import java.util.ArrayList;
import java.util.List;
import java.util.UUID;

import static content.common.annotation.MustExist.Reason.HIBERNATE;
import static javax.persistence.CascadeType.ALL;
import static javax.persistence.FetchType.LAZY;

@Entity
@Table(name = "workout")
public class Workout extends AbstractEntity {

    @Id
    @Column(name = "workout_id")
    @GeneratedValue(generator = "UUID")
    @GenericGenerator(name = "UUID", strategy = "org.hibernate.id.UUIDGenerator")
    private UUID id;

    // userId
    @Column(name = "creator_id")
    private UUID creatorId;

    @Column(name = "description", nullable = false)
    private String description;

    @Column(name = "video_url")
    private String videoUrl;

    @OneToMany(mappedBy = "workout", fetch = LAZY, cascade = ALL, orphanRemoval = true)
    private final List<WorkoutExercise> workoutExercises = new ArrayList<>();

    @ManyToMany(fetch = LAZY)
    @JoinTable(
            name = "workout_body_part",
            joinColumns = @JoinColumn(name = "workout_id"),
            inverseJoinColumns = @JoinColumn(name = "body_part_id")
    )
    private final List<BodyPart> bodyParts = new ArrayList<>();


    @MustExist(reason = HIBERNATE)
    public Workout() {
    }

    public Workout(
            UUID creatorId,
            String description,
            String videoUrl) {

        Assert.notNull(creatorId, "creatorId must not be null");
        Assert.notNull(description, "description must not be null");

        this.creatorId = creatorId;
        this.description = description;
        this.videoUrl = videoUrl;
    }


    @Override
    protected UUID getId() {
        return id;
    }

    public UUID getCreatorId() {
        return creatorId;
    }

    public String getDescription() {
        return description;
    }

    public String getVideoUrl() {
        return videoUrl;
    }

    public List<BodyPart> getBodyParts() {
        return bodyParts;
    }

    public void addBodyPart(BodyPart bodyPart) {

        bodyPart.addWorkout(this);
        bodyParts.add(bodyPart);
    }

    public void removeBodyPart(BodyPart bodyPart) {

        bodyPart.removeWorkout(this);
        bodyParts.remove(bodyPart);
    }
}