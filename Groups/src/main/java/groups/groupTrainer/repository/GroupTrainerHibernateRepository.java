package groups.groupTrainer.repository;

import groups.common.abstracts.AbstractHibernateRepository;
import groups.groupTrainer.entity.GroupTrainer;
import org.hibernate.SessionFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Primary;
import org.springframework.stereotype.Repository;
import org.springframework.util.Assert;

import javax.persistence.criteria.CriteriaBuilder;
import javax.persistence.criteria.CriteriaDelete;
import javax.persistence.criteria.Root;
import java.util.UUID;

import static groups.common.stringUtils.StringUtils.concatenate;

@Primary
@Repository
public class GroupTrainerHibernateRepository extends AbstractHibernateRepository<GroupTrainer> implements GroupTrainerRepository {

    @Autowired
    private GroupTrainerHibernateRepository(SessionFactory sessionFactory) {

        super(GroupTrainer.class, sessionFactory);
    }

    @Override
    public UUID assign(GroupTrainer groupTrainer) {

        Assert.notNull(groupTrainer, "groupTrainer must not be null");

        return save(groupTrainer);
    }

    @Override
    public void unassign(UUID groupTrainerId) {

        Assert.notNull(groupTrainerId, "groupTrainerId must not be null");

        delete(groupTrainerId);
    }

    @Override
    public void unassign(UUID trainerId, UUID groupId) {

        Assert.notNull(trainerId, "trainerId must not be null");
        Assert.notNull(groupId, "groupId must not be null");

        CriteriaBuilder criteriaBuilder = getSession().getCriteriaBuilder();
        CriteriaDelete<GroupTrainer> criteriaDelete = criteriaBuilder.createCriteriaDelete(GroupTrainer.class);
        Root<GroupTrainer> root = criteriaDelete.from(GroupTrainer.class);

        criteriaDelete.where(
                criteriaBuilder.equal(root.get("user_id"), trainerId),
                criteriaBuilder.equal(root.get("group.id"), groupId)
        );

        getSession().createQuery(criteriaDelete)
            .setComment(
                concatenate("Delete GroupTrainer with trainer id = ",
                        trainerId.toString(),
                        " and groupId = ",
                        groupId.toString()
                )
            )
            .executeUpdate();
    }

    @Override
    public void unassignAllByGroupId(UUID groupId) {

        Assert.notNull(groupId, "groupId must not be null");

        CriteriaBuilder criteriaBuilder = getSession().getCriteriaBuilder();
        CriteriaDelete<GroupTrainer> criteriaDelete = criteriaBuilder.createCriteriaDelete(GroupTrainer.class);
        Root<GroupTrainer> root = criteriaDelete.from(GroupTrainer.class);

        criteriaDelete.where(
                criteriaBuilder.equal(root.get("group.id"), groupId)
        );

        getSession().createQuery(criteriaDelete)
                .setComment(
                        concatenate("Delete GroupTrainer with groupId = ", groupId.toString())
                )
                .executeUpdate();
    }
}
