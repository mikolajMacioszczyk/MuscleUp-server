package groups.workout.repository;

import groups.common.abstracts.AbstractHibernateRepository;
import groups.workout.entity.GroupWorkout;
import org.hibernate.SessionFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Primary;
import org.springframework.stereotype.Repository;

@Primary
@Repository
public class GroupWorkoutHibernateRepository extends AbstractHibernateRepository<GroupWorkout> implements GroupWorkoutRepository {

    @Autowired
    protected GroupWorkoutHibernateRepository(SessionFactory sessionFactory) {

        super(GroupWorkout.class, sessionFactory);
    }
}
