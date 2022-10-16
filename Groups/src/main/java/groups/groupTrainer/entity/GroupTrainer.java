package groups.groupTrainer.entity;

import groups.common.abstracts.AbstractEntity;
import groups.common.annotation.MustExist;
import groups.common.annotation.Reason;
import groups.common.annotation.UnknownForeignKey;
import groups.group.entity.Group;
import org.springframework.util.Assert;

import javax.persistence.*;
import java.util.UUID;

@Entity
@Table(name = "class_trainer")
public class GroupTrainer extends AbstractEntity {

    @Id
    private UUID id;

    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "class_id")
    private Group group;

    @UnknownForeignKey
    @Column(name = "user_id", nullable = false)
    private UUID userId;


    @MustExist(reason = Reason.HIBERNATE)
    public GroupTrainer() {
    }


    public GroupTrainer(Group group, UUID userId) {

        Assert.notNull(group, "group must not be null");
        Assert.notNull(userId, "userId must not be null");

        this.group = group;
        this.userId = userId;
    }


    public void update(Group group, UUID userId) {

        Assert.notNull(group, "group must not be null");
        Assert.notNull(userId, "userId must not be null");

        this.group = group;
        this.userId = userId;
    }

    @Override
    public UUID getId() {
        return id;
    }

    public Group getGroup() {
        return group;
    }

    public UUID getUserId() {
        return userId;
    }
}
