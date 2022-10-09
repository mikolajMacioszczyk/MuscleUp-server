package groups.group.entity;

import groups.common.abstracts.AbstractEntity;
import groups.common.annotation.MustExist;
import groups.common.annotation.Reason;
import org.springframework.util.Assert;

import javax.persistence.*;

import static javax.persistence.GenerationType.AUTO;

@Entity
@Table(name = "squad")
public class Group extends AbstractEntity {

    @Id
    @Column(name = "group_id")
    @GeneratedValue(strategy = AUTO)
    private Long id;

    @Column(nullable = false)
    private String name;

    @Column(nullable = false)
    private Long maxParticipants;


    @MustExist(reason = Reason.HIBERNATE)
    public Group() {
    }

    public Group(Long id, String name, Long maxParticipants) {

        Assert.notNull(id, "id must not be null");
        Assert.notNull(name, "name must not be null");
        Assert.notNull(maxParticipants, "maxParticipants must not be null");

        this.id = id;
        this.name = name;
        this.maxParticipants = maxParticipants;
    }


    @Override
    public Long getId() {
        return id;
    }

    public String getName() {
        return name;
    }

    public Long getMaxParticipants() {
        return maxParticipants;
    }
}
