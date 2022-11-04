package groups.group.entity;

import groups.common.abstracts.AbstractEntity;
import groups.common.annotation.MustExist;
import groups.common.annotation.Reason;
import groups.groupPermission.entity.GroupPermission;
import groups.groupTrainer.entity.GroupTrainer;
import groups.groupWorkout.entity.GroupWorkout;
import org.hibernate.annotations.GenericGenerator;
import org.springframework.util.Assert;

import javax.persistence.*;

import java.time.LocalDateTime;
import java.util.List;
import java.util.UUID;

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

    @Column(name = "name", nullable = false)
    private String name;

    @Column(name = "description")
    private String description;

    @Column(name="start_time", nullable = false)
    private LocalDateTime startTime;

    @Column(name="end_time", nullable = false)
    private LocalDateTime endTime;

    @Column(name="repeatable", nullable = false)
    private boolean repeatable;

    @OneToMany(mappedBy = "group", fetch = LAZY, cascade = ALL, orphanRemoval = true)
    private List<GroupPermission> groupPermissions;

    @OneToMany(mappedBy = "group", fetch = LAZY, cascade = ALL, orphanRemoval = true)
    private List<GroupWorkout> groupWorkouts;

    @OneToOne(mappedBy = "group", fetch = LAZY, cascade = ALL, orphanRemoval = true)
    private GroupTrainer groupTrainer;


    @MustExist(reason = Reason.HIBERNATE)
    public Group() {
    }

    public Group(String name, String description, LocalDateTime startTime, LocalDateTime endTime, boolean repeatable) {

        Assert.notNull(name, "name must not be null");
        Assert.notNull(description, "description must not be null");
        Assert.notNull(startTime, "startTime must not be null");
        Assert.notNull(endTime, "endTime must not be null");

        this.name = name;
        this.description = description;
        this.startTime = startTime;
        this.endTime = endTime;
        this.repeatable = repeatable;
    }


    public void update(String name, String description, LocalDateTime startTime, LocalDateTime endTime, boolean repeatable) {

        Assert.notNull(name, "name must not be null");
        Assert.notNull(description, "description must not be null");
        Assert.notNull(startTime, "startTime must not be null");
        Assert.notNull(endTime, "endTime must not be null");

        this.name = name;
        this.description = description;
        this.startTime = startTime;
        this.endTime = endTime;
        this.repeatable = repeatable;
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

    public LocalDateTime getStartTime() {
        return startTime;
    }

    public LocalDateTime getEndTime() {
        return endTime;
    }

    public boolean isRepeatable() {
        return repeatable;
    }

    public List<GroupPermission> getGroupPermissions() {
        return groupPermissions;
    }

    public GroupTrainer getGroupTrainer() {
        return groupTrainer;
    }

    public List<GroupWorkout> getGroupWorkouts() {
        return groupWorkouts;
    }
}
