package content.workout.repository;

import content.common.abstracts.AbstractHibernateQuery;
import content.workout.entity.*;
import org.hibernate.SessionFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Primary;
import org.springframework.stereotype.Repository;
import org.springframework.util.Assert;

import java.util.List;
import java.util.Optional;
import java.util.UUID;

import static java.util.Objects.isNull;
import static java.util.Optional.empty;
import static java.util.Optional.of;


@Primary
@Repository
public class WorkoutHibernateQuery extends AbstractHibernateQuery<Workout> implements WorkoutQuery {

    private final WorkoutDtoFactory workoutDtoFactory;
    private final WorkoutComparisonDtoFactory workoutComparisonDtoFactory;


    @Autowired
    WorkoutHibernateQuery(SessionFactory sessionFactory, WorkoutDtoFactory workoutDtoFactory) {

        super(Workout.class, sessionFactory);

        Assert.notNull(workoutDtoFactory, "workoutDtoFactory must not be null");

        this.workoutDtoFactory = workoutDtoFactory;
        this.workoutComparisonDtoFactory = new WorkoutComparisonDtoFactory();
    }


    @Override
    public WorkoutComparisonDto getForComparison(UUID id) {

        return workoutComparisonDtoFactory.create(getById(id));
    }

    @Override
    public Optional<WorkoutDto> findById(UUID id, UUID fitnessClubId) {

        Assert.notNull(id, "id must not be null");

        Workout workout = getById(id);

        if (isNull(workout)) return empty();

        if (!workout.getFitnessClubId().equals(fitnessClubId)) return empty();

        return of(workoutDtoFactory.create(workout, null));
    }

    @Override
    public List<WorkoutDto> getAllWorkouts(UUID fitnessClubId) {

        return getAll().stream()
                .filter(workout -> workout.getFitnessClubId().equals(fitnessClubId))
                .map(workout -> workoutDtoFactory.create(workout, null))
                .toList();
    }

    @Override
    public List<WorkoutDto> getAllActiveWorkouts(UUID fitnessClubId) {

        return getAll().stream()
                .filter(workout -> workout.getFitnessClubId().equals(fitnessClubId))
                .filter(Workout::isActive)
                .map(workout -> workoutDtoFactory.create(workout, null))
                .toList();
    }
}
