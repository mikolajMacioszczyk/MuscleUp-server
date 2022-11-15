package content.workoutExerciseCriterionResult.repository;

import content.common.abstracts.AbstractHibernateRepository;
import content.workoutExerciseCriterionResult.entity.WorkoutExerciseCriterionResult;
import org.hibernate.Session;
import org.hibernate.SessionFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Primary;
import org.springframework.stereotype.Repository;
import org.springframework.util.Assert;

import javax.persistence.criteria.CriteriaBuilder;
import javax.persistence.criteria.CriteriaQuery;
import javax.persistence.criteria.Root;
import javax.transaction.Transactional;
import java.util.UUID;

@Primary
@Repository
public class WorkoutExerciseCriterionResultHibernateExerciseCriterionResultRepository extends AbstractHibernateRepository<WorkoutExerciseCriterionResult> implements WorkoutExerciseCriterionResultRepository {

    @Autowired
    WorkoutExerciseCriterionResultHibernateExerciseCriterionResultRepository(SessionFactory sessionFactory) {

        super(WorkoutExerciseCriterionResult.class, sessionFactory);
    }

    @Override
    public WorkoutExerciseCriterionResult getByParams(UUID userId, UUID workoutExerciseId, UUID criterionId) {

        Assert.notNull(userId, "userId must not be null");
        Assert.notNull(workoutExerciseId, "workoutExerciseId must not be null");
        Assert.notNull(criterionId, "criterionId must not be null");

        CriteriaBuilder criteriaBuilder = getSession().getCriteriaBuilder();
        CriteriaQuery<WorkoutExerciseCriterionResult> criteriaQuery = criteriaBuilder.createQuery(WorkoutExerciseCriterionResult.class);
        Root<WorkoutExerciseCriterionResult> root = criteriaQuery.from(WorkoutExerciseCriterionResult.class);

        criteriaQuery.multiselect(
                root.get("id"),
                root.get("userId"),
                root.get("workoutExercise"),
                root.get("criterion"),
                root.get("value"),
                root.get("performedWorkoutId")
        ).where(
                criteriaBuilder.equal(root.get("userId"), userId),
                criteriaBuilder.equal(root.get("workoutExercise").get("id"), workoutExerciseId),
                criteriaBuilder.equal(root.get("criterion").get("id"), criterionId)
        );

        return getSession().createQuery(criteriaQuery)
                .getSingleResult();
    }

    @Transactional
    @Override
    public UUID merge(WorkoutExerciseCriterionResult workout) {

        Assert.notNull(workout, "workout must not be null");

        Session session = getSession();

        session.merge(WorkoutExerciseCriterionResult.class.getName(), workout);

        return workout.getId();
    }
}