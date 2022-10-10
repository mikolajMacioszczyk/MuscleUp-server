package groups.group.entity;

import groups.common.abstracts.AbstractEntity;
import groups.common.annotation.MustExist;
import groups.common.annotation.Reason;
import org.hibernate.annotations.GenericGenerator;
import org.springframework.lang.Nullable;
import org.springframework.util.Assert;

import javax.persistence.*;

import java.util.UUID;

@Entity
@Table(name = "squad")
public class Group extends AbstractEntity {

    @Id
    @Column(name = "group_id")
    @GeneratedValue(generator = "UUID")
    @GenericGenerator(name = "UUID", strategy = "org.hibernate.id.UUIDGenerator")
    private UUID id;

    @Column(nullable = false)
    private String name;

    @Column(nullable = false)
    private Long maxParticipants;


    @MustExist(reason = Reason.HIBERNATE)
    public Group() {
    }

    public Group(@Nullable UUID id, String name, Long maxParticipants) {

        Assert.notNull(name, "name must not be null");
        Assert.notNull(maxParticipants, "maxParticipants must not be null");

        this.id = id;
        this.name = name;
        this.maxParticipants = maxParticipants;
    }


    public void update(String name, Long maxParticipants) {

        Assert.notNull(name, "name must not be null");
        Assert.notNull(maxParticipants, "maxParticipants must not be null");

        this.name = name;
        this.maxParticipants = maxParticipants;
    }

    @Override
    public UUID getId() {
        return id;
    }

    public String getName() {
        return name;
    }

    public Long getMaxParticipants() {
        return maxParticipants;
    }
}
