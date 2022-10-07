package groups.group.entity;

import groups.common.abstracts.AbstractEntity;
import groups.common.annotation.MustExist;
import groups.common.annotation.Reason;
import groups.groupWorkout.entity.GroupWorkout;
import org.springframework.util.Assert;

import javax.persistence.*;
import java.util.HashSet;
import java.util.Set;

import static javax.persistence.CascadeType.ALL;

@Entity
@Table(name = "group")
public class Group extends AbstractEntity {

    @Id
    @Column(name = "group_id")
    private String id;

    @Column(name = "group_name", nullable = false)
    private String name;

    @Column(nullable = false)
    private Long maxParticipants;

    @OneToMany(mappedBy = "groupWorkout",  cascade = ALL, orphanRemoval = true, fetch = FetchType.LAZY)
    private Set<GroupWorkout> recordings;


    @MustExist(reason = Reason.HIBERNATE)
    public Group() {
    }

    public Group(String id, String name, Long maxParticipants) {

        Assert.notNull(id, "id must not be null");
        Assert.notNull(name, "name must not be null");
        Assert.notNull(maxParticipants, "maxParticipants must not be null");

        this.id = id;
        this.name = name;
        this.maxParticipants = maxParticipants;
    }


    @Override
    public String getId() {
        return id;
    }

    public String getName() {
        return name;
    }

    public Long getMaxParticipants() {
        return maxParticipants;
    }
}
