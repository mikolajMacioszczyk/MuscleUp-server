package groups.workoutParticipant.repository;

import groups.common.abstracts.AbstractHibernateQuery;
import groups.workoutParticipant.entity.WorkoutParticipant;
import groups.workoutParticipant.entity.WorkoutParticipantDto;
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

import static groups.common.utils.StringUtils.concatenate;

@Primary
@Repository
public class WorkoutParticipantHibernateQuery extends AbstractHibernateQuery<WorkoutParticipant> implements WorkoutParticipantQuery {


    @Autowired
    WorkoutParticipantHibernateQuery(SessionFactory sessionFactory) {

        super(WorkoutParticipant.class, sessionFactory);
    }


    @Override
    @Transactional
    public List<WorkoutParticipantDto> getAllWorkoutParticipantsByGroupWorkoutIdAndUserId(UUID groupWorkoutId, UUID userId) {

        Assert.notNull(groupWorkoutId, "groupWorkoutId must not be null");
        Assert.notNull(userId, "userId must not be null");

        CriteriaBuilder criteriaBuilder = getSession().getCriteriaBuilder();
        CriteriaQuery<WorkoutParticipantDto> criteriaQuery = criteriaBuilder.createQuery(WorkoutParticipantDto.class);
        Root<WorkoutParticipant> root = criteriaQuery.from(WorkoutParticipant.class);

        criteriaQuery.multiselect(
                root.get("groupWorkout").get("id"),
                root.get("userId")
        ).where(
                criteriaBuilder.equal(root.get("groupWorkout").get("id"), groupWorkoutId),
                criteriaBuilder.equal(root.get("userId"), userId)
        );

        return getSession().createQuery(criteriaQuery)
                .setComment(
                        concatenate("WorkoutParticipant with groupWorkoutId = ",
                                groupWorkoutId.toString(),
                                " and userId =",
                                userId.toString()
                        )
                )
                .getResultList();
    }
}
