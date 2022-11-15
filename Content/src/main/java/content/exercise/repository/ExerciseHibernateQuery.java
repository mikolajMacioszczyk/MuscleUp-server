package content.exercise.repository;

import content.common.abstracts.AbstractHibernateQuery;
import content.criterion.entity.Criterion;
import content.exercise.entity.Exercise;
import content.exercise.entity.ExerciseDto;
import content.exercise.entity.ExerciseDtoFactory;
import org.hibernate.SessionFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Primary;
import org.springframework.stereotype.Repository;
import org.springframework.util.Assert;

import javax.transaction.Transactional;
import java.util.List;
import java.util.Optional;
import java.util.UUID;

import static java.util.Objects.isNull;
import static java.util.Optional.empty;
import static java.util.Optional.of;


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
    public ExerciseDto get(UUID id) {

        return exerciseDtoFactory.create(getById(id));
    }

    @Override
    @Transactional
    public Optional<ExerciseDto> findById(UUID id, UUID fitnessClubId) {

        Assert.notNull(id, "id must not be null");

        Exercise exercise = getById(id);

        if (isNull(exercise)) return empty();

        if (!exercise.getFitnessClubId().equals(fitnessClubId)) return empty();

        return of(exerciseDtoFactory.create(exercise));
    }

    @Override
    public List<ExerciseDto> getAllExercises(UUID fitnessClubId) {

        return getAll().stream()
                .filter(exercise -> exercise.getFitnessClubId().equals(fitnessClubId))
                .map(exerciseDtoFactory::create)
                .toList();
    }

    @Override
    public List<ExerciseDto> getAllActiveExercises(UUID fitnessClubId) {

        return getAll().stream()
                .filter(exercise -> exercise.getFitnessClubId().equals(fitnessClubId))
                .filter(Exercise::isLatest)
                .map(exerciseDtoFactory::create)
                .toList();
    }

    @Override
    public List<UUID> getAllAppliedCriteriaById(UUID id) {

        return getAll().stream()
                .filter(exercise -> exercise.getId().equals(id))
                .findFirst()
                .get()
                .getCriteria()
                .stream()
                .map(Criterion::getId)
                .toList();
    }
}
