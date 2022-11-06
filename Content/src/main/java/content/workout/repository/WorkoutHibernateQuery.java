package content.workout.repository;

import content.bodyPart.entity.BodyPart;
import content.common.abstracts.AbstractHibernateQuery;
import content.workout.entity.Workout;
import content.workout.entity.WorkoutDto;
import content.workout.entity.WorkoutDtoFactory;
import org.hibernate.SessionFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Primary;
import org.springframework.stereotype.Repository;
import org.springframework.util.Assert;

import java.util.List;
import java.util.Optional;
import java.util.UUID;
import java.util.stream.Collectors;

import static java.util.Objects.isNull;
import static java.util.Optional.empty;
import static java.util.Optional.of;


@Primary
@Repository
public class WorkoutHibernateQuery extends AbstractHibernateQuery<Workout> implements WorkoutQuery {

    private final WorkoutDtoFactory workoutDtoFactory;


    @Autowired
    WorkoutHibernateQuery(SessionFactory sessionFactory) {

        super(Workout.class, sessionFactory);

        this.workoutDtoFactory = new WorkoutDtoFactory();
    }


    @Override
    public Optional<WorkoutDto> findById(UUID id) {

        Assert.notNull(id, "id must not be null");

        Workout workout = getById(id);

        return isNull(workout)? empty() :
                of(workoutDtoFactory.create(workout));
    }

    @Override
    public List<WorkoutDto> getAllWorkouts() {

        return getAll().stream()
                .map(workoutDtoFactory::create)
                .collect(Collectors.toList());
    }

    @Override
    public List<UUID> getBodyPartsByWorkoutId(UUID id) {

        Assert.notNull(id, "id must not be null");

        Workout workout = getById(id);

        return workout.getBodyParts()
                .stream()
                .map(BodyPart::getId)
                .toList();
    }
}
