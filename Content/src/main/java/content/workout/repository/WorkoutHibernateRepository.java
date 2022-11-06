package content.workout.repository;

import content.common.abstracts.AbstractHibernateRepository;
import content.workout.entity.Workout;
import org.hibernate.SessionFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Primary;
import org.springframework.stereotype.Repository;

@Primary
@Repository
public class WorkoutHibernateRepository extends AbstractHibernateRepository<Workout> implements WorkoutRepository {

    @Autowired
    WorkoutHibernateRepository(SessionFactory sessionFactory) {

        super(Workout.class, sessionFactory);
    }
}