package content.exercise.repository;

import content.common.abstracts.AbstractHibernateQuery;
import content.exercise.entity.Exercise;
import content.exercise.entity.ExerciseDto;
import content.exercise.entity.ExerciseDtoFactory;
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
import java.util.Optional;
import java.util.UUID;
import java.util.stream.Collectors;


@Primary
@Repository
public class ExerciseHibernateQuery extends AbstractHibernateQuery<Exercise> implements ExerciseQuery {

    private final ExerciseDtoFactory exerciseDtoFactory;


    @Autowired
    ExerciseHibernateQuery(SessionFactory sessionFactory) {

        super(Exercise.class, sessionFactory);

        this.exerciseDtoFactory = new ExerciseDtoFactory();
    }


    @Override
    @Transactional
    public Optional<ExerciseDto> findById(UUID id) {

        Assert.notNull(id, "id must not be null");

        CriteriaBuilder criteriaBuilder = getSession().getCriteriaBuilder();
        CriteriaQuery<ExerciseDto> criteriaQuery = criteriaBuilder.createQuery(ExerciseDto.class);
        Root<Exercise> root = criteriaQuery.from(Exercise.class);

        criteriaQuery.multiselect(
                root.get("id"),
                root.get("name"),
                root.get("description"),
                root.get("imageUrl"),
                root.get("videoUrl")
        ).where(
                criteriaBuilder.equal(root.get("id"), id)
        );

        return getSession().createQuery(criteriaQuery)
                .setComment("Workout with id = " +  id)
                .getResultList()
                .stream()
                .findFirst();
    }

    @Override
    public List<ExerciseDto> getAllExercises() {

        return getAll().stream()
                .map(exerciseDtoFactory::create)
                .collect(Collectors.toList());
    }
}
