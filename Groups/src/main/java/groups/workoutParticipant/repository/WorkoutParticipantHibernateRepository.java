package groups.workoutParticipant.repository;

import groups.common.abstracts.AbstractHibernateRepository;
import groups.workoutParticipant.entity.WorkoutParticipant;
import org.hibernate.SessionFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Primary;
import org.springframework.stereotype.Repository;
import org.springframework.util.Assert;

import javax.persistence.criteria.CriteriaBuilder;
import javax.persistence.criteria.CriteriaDelete;
import javax.persistence.criteria.Root;
import javax.transaction.Transactional;
import java.util.UUID;

import static groups.common.utils.StringUtils.concatenate;

@Primary
@Repository
public class WorkoutParticipantHibernateRepository extends AbstractHibernateRepository<WorkoutParticipant> implements WorkoutParticipantRepository {


    @Autowired
    WorkoutParticipantHibernateRepository(SessionFactory sessionFactory) {

        super(WorkoutParticipant.class, sessionFactory);
    }


    @Override
    @Transactional
    public UUID assign(WorkoutParticipant workoutParticipant) {

        Assert.notNull(workoutParticipant, "workoutParticipant must not be null");

        return save(workoutParticipant);
    }

    @Override
    @Transactional
    public void unassign(UUID groupWorkoutId, UUID userId) {

        Assert.notNull(groupWorkoutId, "groupWorkoutId must not be null");
        Assert.notNull(userId, "userId must not be null");

        CriteriaBuilder criteriaBuilder = getSession().getCriteriaBuilder();
        CriteriaDelete<WorkoutParticipant> criteriaDelete = criteriaBuilder.createCriteriaDelete(WorkoutParticipant.class);
        Root<WorkoutParticipant> root = criteriaDelete.from(WorkoutParticipant.class);

        criteriaDelete.where(
                criteriaBuilder.equal(root.get("userId"), userId),
                criteriaBuilder.equal(root.get("groupWorkout").get("id"), groupWorkoutId)
        );

        getSession().createQuery(criteriaDelete)
                .setComment(
                        concatenate("Delete WorkoutParticipant with GroupWorkoutId = ",
                                groupWorkoutId.toString(),
                                " and UserId = ",
                                userId.toString()
                        )
                )
                .executeUpdate();
    }
}

