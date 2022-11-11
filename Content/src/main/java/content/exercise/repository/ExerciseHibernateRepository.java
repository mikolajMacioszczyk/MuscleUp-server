package content.exercise.repository;

import content.common.abstracts.AbstractHibernateRepository;
import content.exercise.entity.Exercise;
import org.hibernate.SessionFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Primary;
import org.springframework.stereotype.Repository;

@Primary
@Repository
public class ExerciseHibernateRepository extends AbstractHibernateRepository<Exercise> implements ExerciseRepository {

    @Autowired
    ExerciseHibernateRepository(SessionFactory sessionFactory) {

        super(Exercise.class, sessionFactory);
    }
}