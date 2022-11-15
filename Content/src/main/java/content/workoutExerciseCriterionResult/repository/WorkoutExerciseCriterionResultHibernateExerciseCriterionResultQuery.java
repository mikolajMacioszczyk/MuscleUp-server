package content.workoutExerciseCriterionResult.repository;

import content.common.abstracts.AbstractHibernateQuery;
import content.workoutExerciseCriterionResult.entity.WorkoutExerciseCriterionResult;
import org.hibernate.SessionFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Primary;
import org.springframework.stereotype.Repository;

import java.util.UUID;

import static java.util.Objects.isNull;


@Primary
@Repository
public class WorkoutExerciseCriterionResultHibernateExerciseCriterionResultQuery extends AbstractHibernateQuery<WorkoutExerciseCriterionResult> implements WorkoutExerciseCriterionResultQuery {


    @Autowired
    WorkoutExerciseCriterionResultHibernateExerciseCriterionResultQuery(SessionFactory sessionFactory) {

        super(WorkoutExerciseCriterionResult.class, sessionFactory);
    }


    @Override
    public int getDefaultValue(UUID userId, UUID workoutExerciseId, UUID criterionId) {

        return getAll().stream()
                .filter(result -> result.getUserId().equals(userId))
                .filter(result -> result.getWorkoutExerciseId().equals(workoutExerciseId))
                .filter(result -> result.getCriterionId().equals(criterionId))
                .filter(result -> result.getPerformedWorkoutId() == null)
                .findFirst().get().getValue();
    }

    @Override
    public int getValue(UUID userId, UUID workoutExerciseId, UUID criterionId, UUID performedWorkoutId) {

        if (isNull(performedWorkoutId)) {

            return getDefaultValue(userId, workoutExerciseId, criterionId);
        }
        else {

            return getAll().stream()
                    .filter(result -> result.getUserId().equals(userId))
                    .filter(result -> result.getWorkoutExerciseId().equals(workoutExerciseId))
                    .filter(result -> result.getCriterionId().equals(criterionId))
                    .filter(result -> result.getPerformedWorkoutId().equals(performedWorkoutId))
                    .findFirst().get().getValue();
        }
    }
}
