package groups.group.entity;

import groups.common.abstracts.AbstractEntity;
import groups.common.annotation.MustExist;
import groups.common.annotation.UnknownForeignKey;
import groups.groupPermission.entity.GroupPermission;
import groups.groupWorkout.entity.GroupWorkout;
import org.hibernate.annotations.GenericGenerator;
import org.springframework.lang.Nullable;
import org.springframework.util.Assert;

import javax.persistence.*;

import java.util.List;
import java.util.UUID;

import static groups.common.annotation.MustExist.Reason.HIBERNATE;
import static javax.persistence.CascadeType.ALL;
import static javax.persistence.FetchType.LAZY;

@Entity
@Table(name = "class")
public class Group extends AbstractEntity {

    @Id
    @Column(name = "class_id")
    @GeneratedValue(generator = "UUID")
    @GenericGenerator(name = "UUID", strategy = "org.hibernate.id.UUIDGenerator")
    private UUID id;

    @UnknownForeignKey
    @Column(name = "trainer_id", nullable = false)
    private UUID trainerId;

    @UnknownForeignKey
    @Column(name = "fitness_club_id", nullable = false)
    private UUID fitnessClubId;

    @Column(name = "name", nullable = false)
    private String name;

    @Column(name = "description")
    private String description;

    @Column(name = "location", nullable = false)
    private String location;

    @Column(name = "max_participants", nullable = false)
    private int maxParticipants;

    @Column(name = "repeatable", nullable = false)
    private boolean repeatable;

    @OneToMany(mappedBy = "group", fetch = LAZY, cascade = ALL, orphanRemoval = true)
    private List<GroupPermission> groupPermissions;

    @OneToMany(mappedBy = "group", fetch = LAZY, cascade = ALL, orphanRemoval = true)
    private List<GroupWorkout> groupWorkouts;


    @MustExist(reason = HIBERNATE)
    public Group() {
    }

    public Group(String name,
                 UUID trainerId,
                 UUID fitnessClubId,
                 @Nullable String description,
                 String location,
                 int maxParticipants,
                 boolean repeatable) {

        Assert.notNull(name, "name must not be null");
        Assert.notNull(trainerId, "trainerId must not be null");
        Assert.notNull(fitnessClubId, "fitnessClubId must not be null");
        Assert.notNull(location, "location must not be null");

        this.name = name;
        this.trainerId = trainerId;
        this.fitnessClubId = fitnessClubId;
        this.description = description;
        this.location = location;
        this.maxParticipants = maxParticipants;
        this.repeatable = repeatable;
    }


    public void update(String name,
                       UUID trainerId,
                       UUID fitnessClubId,
                       @Nullable String description,
                       String location,
                       int maxParticipants,
                       boolean repeatable) {

        Assert.notNull(name, "name must not be null");
        Assert.notNull(trainerId, "trainerId must not be null");
        Assert.notNull(fitnessClubId, "fitnessClubId must not be null");
        Assert.notNull(location, "location must not be null");

        this.name = name;
        this.trainerId = trainerId;
        this.fitnessClubId = fitnessClubId;
        this.description = description;
        this.location = location;
        this.maxParticipants = maxParticipants;
        this.repeatable = repeatable;
    }

    @Override
    public UUID getId() {
        return id;
    }

    public UUID getTrainerId() {
        return trainerId;
    }

    public UUID getFitnessClubId() {
        return fitnessClubId;
    }

    public String getName() {
        return name;
    }

    public String getDescription() {
        return description;
    }

    public String getLocation() {
        return location;
    }

    public int getMaxParticipants() {
        return maxParticipants;
    }

    public boolean isRepeatable() {
        return repeatable;
    }

    public List<GroupPermission> getGroupPermissions() {
        return groupPermissions;
    }
}
