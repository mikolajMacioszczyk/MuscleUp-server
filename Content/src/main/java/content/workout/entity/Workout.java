package content.workout.entity;

import content.bodyPart.entity.BodyPart;
import content.common.abstracts.AbstractEntity;
import content.common.annotation.MustExist;
import org.hibernate.annotations.GenericGenerator;
import org.springframework.util.Assert;

import javax.persistence.*;
import java.util.ArrayList;
import java.util.List;
import java.util.UUID;

import static content.common.annotation.MustExist.Reason.HIBERNATE;
import static javax.persistence.FetchType.LAZY;

@Entity
@Table(name = "workout")
public class Workout extends AbstractEntity {

    @Id
    @Column(name = "workout_id")
    @GeneratedValue(generator = "UUID")
    @GenericGenerator(name = "UUID", strategy = "org.hibernate.id.UUIDGenerator")
    private UUID id;

    @Column(name = "description", nullable = false)
    private String description;

    @Column(name = "video_url")
    private String videoUrl;

    @Column(name = "expected_perform_time")
    private Long expectedPerformTime;

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

    public Workout(String description, String videoUrl, Long expectedPerformTime) {

        Assert.notNull(description, "description must not be null");

        this.description = description;
        this.videoUrl = videoUrl;
        this.expectedPerformTime = expectedPerformTime;
    }


    public void update(String description, String videoUrl, Long expectedPerformTime) {

        Assert.notNull(description, "description must not be null");

        this.description = description;
        this.videoUrl = videoUrl;
        this.expectedPerformTime = expectedPerformTime;
    }


    @Override
    protected UUID getId() {
        return id;
    }

    public String getDescription() {
        return description;
    }

    public String getVideoUrl() {
        return videoUrl;
    }

    public Long getExpectedPerformTime() {
        return expectedPerformTime;
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