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
import static java.util.Collections.sort;
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
    private List<WorkoutExercise> workoutExercises = new ArrayList<>();

    @ManyToMany(fetch = LAZY)
    @JoinTable(
            name = "workout_body_part",
            joinColumns = @JoinColumn(name = "workout_id"),
            inverseJoinColumns = @JoinColumn(name = "body_part_id")
    )
    private List<BodyPart> bodyParts = new ArrayList<>();


    @MustExist(reason = HIBERNATE)
    public Workout() {
    }

    public Workout(
            UUID creatorId,
            String description,
            String videoUrl,
            List<WorkoutExercise> workoutExercises,
            List<BodyPart> bodyParts) {

        Assert.notNull(creatorId, "creatorId must not be null");
        Assert.notNull(description, "description must not be null");
        Assert.notNull(workoutExercises, "workoutExercises must not be null");
        Assert.notNull(bodyParts, "bodyParts must not be null");
        workoutExercises.forEach(workoutExercise -> Assert.notNull(workoutExercise, "workoutExercise must not be null"));
        bodyParts.forEach(workoutExercise -> Assert.notNull(workoutExercise, "workoutExercise must not be null"));

        this.creatorId = creatorId;
        this.description = description;
        this.videoUrl = videoUrl;
        this.workoutExercises = workoutExercises;
        this.bodyParts = bodyParts;
    }


    @Override
    public UUID getId() {
        return id;
    }

    public void setId(UUID id) {
        this.id = id;
    }

    public UUID getCreatorId() {
        return creatorId;
    }

    public String getDescription() {
        return description;
    }

    public void setDescription(String description) {
        this.description = description;
    }

    public String getVideoUrl() {
        return videoUrl;
    }

    public void setVideoUrl(String videoUrl) {
        this.videoUrl = videoUrl;
    }

    public List<BodyPart> getBodyParts() {
        return bodyParts;
    }

    public List<WorkoutExercise> getWorkoutExercises() {

        sort(workoutExercises);

        return workoutExercises;
    }

    public void addWorkoutExercise(WorkoutExercise workoutExercise) {

        workoutExercises.add(workoutExercise);
        workoutExercise.setWorkout(this);
    }

    public void addBodyPart(BodyPart bodyPart) {

        bodyParts.add(bodyPart);
    }
}