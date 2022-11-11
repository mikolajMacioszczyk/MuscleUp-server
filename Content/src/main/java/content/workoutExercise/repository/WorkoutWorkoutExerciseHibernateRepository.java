package content.workoutExercise.repository;

import content.common.abstracts.AbstractHibernateRepository;
import content.workoutExercise.entity.WorkoutExercise;
import org.hibernate.SessionFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Primary;
import org.springframework.stereotype.Repository;

@Primary
@Repository
public class WorkoutWorkoutExerciseHibernateRepository extends AbstractHibernateRepository<WorkoutExercise> implements WorkoutExerciseRepository {

    @Autowired
    WorkoutWorkoutExerciseHibernateRepository(SessionFactory sessionFactory) {

        super(WorkoutExercise.class, sessionFactory);
    }
}