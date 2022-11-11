package content.workoutExercise.repository;

import content.common.abstracts.AbstractHibernateQuery;
import content.common.wrappers.UuidWrapper;
import content.workoutExercise.entity.WorkoutExercise;
import content.workoutExercise.entity.WorkoutExerciseDtoFactory;
import org.hibernate.SessionFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Primary;
import org.springframework.stereotype.Repository;
import org.springframework.util.Assert;

import javax.persistence.criteria.CriteriaBuilder;
import javax.persistence.criteria.CriteriaQuery;
import javax.persistence.criteria.Root;
import java.util.UUID;


@Primary
@Repository
public class WorkoutWorkoutExerciseHibernateQuery extends AbstractHibernateQuery<WorkoutExercise> implements WorkoutExerciseQuery {

    @Autowired
    WorkoutWorkoutExerciseHibernateQuery(SessionFactory sessionFactory) {

        super(WorkoutExercise.class, sessionFactory);
    }

    @Override
    public boolean isExerciseConnected(UUID exerciseId) {

        Assert.notNull(exerciseId, "exerciseId must not be null");

        CriteriaBuilder criteriaBuilder = getSession().getCriteriaBuilder();
        CriteriaQuery<UuidWrapper> criteriaQuery = criteriaBuilder.createQuery(UuidWrapper.class);
        Root<WorkoutExercise> root = criteriaQuery.from(WorkoutExercise.class);

        criteriaQuery.multiselect(
                root.get("exercise").get("id")
        ).where(
                criteriaBuilder.equal(root.get("exercise").get("id"), exerciseId)
        );

        return !getSession().createQuery(criteriaQuery)
                .setComment("Is exercise connected with any workout")
                .getResultList()
                .isEmpty();
    }
}
