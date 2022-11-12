package content.performedWorkout.repository;

import content.common.abstracts.AbstractHibernateRepository;
import content.performedWorkout.entity.PerformedWorkout;
import org.hibernate.SessionFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Primary;
import org.springframework.stereotype.Repository;

@Primary
@Repository
public class PerformedWorkoutHibernateRepository extends AbstractHibernateRepository<PerformedWorkout> implements PerformedWorkoutRepository {

    @Autowired
    PerformedWorkoutHibernateRepository(SessionFactory sessionFactory) {

        super(PerformedWorkout.class, sessionFactory);
    }
}