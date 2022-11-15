package content.performedWorkout.repository;

import content.common.abstracts.AbstractHibernateQuery;
import content.performedWorkout.entity.PerformedWorkout;
import content.performedWorkout.entity.PerformedWorkoutDto;
import content.performedWorkout.entity.PerformedWorkoutDtoFactory;
import org.hibernate.SessionFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Primary;
import org.springframework.stereotype.Repository;
import org.springframework.util.Assert;

import java.util.List;
import java.util.UUID;



@Primary
@Repository
public class PerformedWorkoutHibernateQuery extends AbstractHibernateQuery<PerformedWorkout> implements PerformedWorkoutQuery {

    private final PerformedWorkoutDtoFactory performedWorkoutDtoFactory;


    @Autowired
    PerformedWorkoutHibernateQuery(SessionFactory sessionFactory, PerformedWorkoutDtoFactory performedWorkoutDtoFactory) {

        super(PerformedWorkout.class, sessionFactory);

        Assert.notNull(performedWorkoutDtoFactory, "performedWorkoutDtoFactory must not be null");

        this.performedWorkoutDtoFactory = performedWorkoutDtoFactory;
    }


    @Override
    public PerformedWorkoutDto get(UUID id) {

        return performedWorkoutDtoFactory.create(getById(id));
    }

    @Override
    public List<PerformedWorkoutDto> getAllPerformedWorkouts() {

        return getAll().stream()
                .map(performedWorkoutDtoFactory::create)
                .toList();
    }

    @Override
    public List<PerformedWorkoutDto> getAllPerformedWorkoutsByUserId(UUID userId) {

        return getAll().stream()
                .filter(performedWorkout -> performedWorkout.getUserId().equals(userId))
                .map(performedWorkoutDtoFactory::create)
                .toList();
    }

    @Override
    public List<PerformedWorkoutDto> getAllPerformedWorkoutsByCreatorId(UUID creatorId) {

        return getAll().stream()
                .filter(performedWorkout -> performedWorkout.getWorkout().getCreatorId().equals(creatorId))
                .map(performedWorkoutDtoFactory::create)
                .toList();
    }

    @Override
    public List<PerformedWorkoutDto> getAllPerformedWorkoutsByWorkoutId(UUID workoutId) {

        return getAll().stream()
                .filter(performedWorkout -> performedWorkout.getWorkout().getId().equals(workoutId))
                .map(performedWorkoutDtoFactory::create)
                .toList();
    }

    @Override
    public Integer getPerformancesByWorkoutId(UUID workoutId) {

        return getAllPerformedWorkoutsByWorkoutId(workoutId).size();
    }
}
