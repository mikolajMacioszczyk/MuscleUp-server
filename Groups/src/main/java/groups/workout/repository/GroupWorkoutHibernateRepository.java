package groups.workout.repository;

import groups.common.wrappers.UuidWrapper;
import groups.common.abstracts.AbstractHibernateRepository;
import groups.workout.entity.GroupWorkout;
import org.hibernate.SessionFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Primary;
import org.springframework.stereotype.Repository;
import org.springframework.util.Assert;

import javax.persistence.criteria.CriteriaBuilder;
import javax.persistence.criteria.CriteriaQuery;
import javax.persistence.criteria.Root;
import javax.transaction.Transactional;
import java.util.List;
import java.util.UUID;

import static groups.common.stringUtils.StringUtils.concatenate;

@Primary
@Repository
public class GroupWorkoutHibernateRepository extends AbstractHibernateRepository<GroupWorkout> implements GroupWorkoutRepository {

    @Autowired
    GroupWorkoutHibernateRepository(SessionFactory sessionFactory) {

        super(GroupWorkout.class, sessionFactory);
    }

    @Override
    @Transactional
    public List<UuidWrapper> getIdsByGroupId(UUID groupId) {

        Assert.notNull(groupId, "groupId must not be null");

        CriteriaBuilder criteriaBuilder = getSession().getCriteriaBuilder();
        CriteriaQuery<UuidWrapper> criteriaQuery = criteriaBuilder.createQuery(UuidWrapper.class);
        Root<GroupWorkout> root = criteriaQuery.from(GroupWorkout.class);

        criteriaQuery.multiselect(
                root.get("id")
        ).where(
                criteriaBuilder.equal(root.get("group").get("id"), groupId)
        );

        return getSession().createQuery(criteriaQuery)
                .setComment(
                        concatenate("GroupWorkout with groupId = ", groupId.toString())
                )
                .getResultList();
    }
}
