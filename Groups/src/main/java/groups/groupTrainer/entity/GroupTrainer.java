package groups.groupTrainer.entity;

import groups.common.abstracts.AbstractEntity;
import groups.common.annotation.MustExist;
import groups.common.annotation.Reason;
import groups.common.annotation.UnknownForeignKey;
import groups.group.entity.Group;
import org.hibernate.annotations.GenericGenerator;
import org.springframework.util.Assert;

import javax.persistence.*;
import java.util.UUID;

import static javax.persistence.FetchType.LAZY;

@Entity
@Table(name = "class_trainer")
public class GroupTrainer extends AbstractEntity {

    @Id
    @Column(name = "class_trainer_id")
    @GeneratedValue(generator = "UUID")
    @GenericGenerator(name = "UUID", strategy = "org.hibernate.id.UUIDGenerator")
    private UUID id;

    @OneToOne(fetch = LAZY)
    @JoinColumn(name = "class_id", nullable = false)
    private Group group;

    @UnknownForeignKey
    @Column(name = "trainer_id", nullable = false)
    private UUID trainerId;


    @MustExist(reason = Reason.HIBERNATE)
    public GroupTrainer() {
    }

    public GroupTrainer(Group group, UUID trainerId) {

        Assert.notNull(group, "group must not be null");
        Assert.notNull(trainerId, "trainerId must not be null");

        this.group = group;
        this.trainerId = trainerId;
    }


    public void update(Group group, UUID userId) {

        Assert.notNull(group, "group must not be null");
        Assert.notNull(userId, "userId must not be null");

        this.group = group;
        this.trainerId = userId;
    }

    @Override
    public UUID getId() {
        return id;
    }

    public Group getGroup() {
        return group;
    }

    public UUID getTrainerId() {
        return trainerId;
    }
}
