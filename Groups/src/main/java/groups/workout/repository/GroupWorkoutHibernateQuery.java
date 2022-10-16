package groups.workout.repository;

import groups.common.abstracts.AbstractHibernateQuery;
import groups.workout.entity.GroupWorkout;
import groups.workout.entity.GroupWorkoutFullDto;
import groups.workout.entity.GroupWorkoutFullDtoFactory;
import org.hibernate.SessionFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Primary;
import org.springframework.stereotype.Repository;
import org.springframework.util.Assert;

import javax.persistence.criteria.CriteriaBuilder;
import javax.persistence.criteria.CriteriaQuery;
import javax.persistence.criteria.Root;
import java.util.List;
import java.util.Optional;
import java.util.UUID;
import java.util.stream.Collectors;

import static groups.common.stringUtils.StringUtils.concatenate;

@Primary
@Repository
public class GroupWorkoutHibernateQuery extends AbstractHibernateQuery<GroupWorkout> implements GroupWorkoutQuery {

    private final GroupWorkoutFullDtoFactory groupWorkoutFullDtoFactory;


    @Autowired
    private GroupWorkoutHibernateQuery(SessionFactory sessionFactory) {

        super(GroupWorkout.class, sessionFactory);

        this.groupWorkoutFullDtoFactory = new GroupWorkoutFullDtoFactory();
    }


    @Override
    public Optional<GroupWorkoutFullDto> findGroupWorkoutById(UUID id) {

        Assert.notNull(id, "id must not be null");

        CriteriaBuilder criteriaBuilder = getSession().getCriteriaBuilder();
        CriteriaQuery<GroupWorkoutFullDto> criteriaQuery = criteriaBuilder.createQuery(GroupWorkoutFullDto.class);
        Root<GroupWorkout> root = criteriaQuery.from(GroupWorkout.class);

        criteriaQuery.multiselect(
                root.get("id"),
                root.get("group.id"),
                root.get("workoutId"),
                root.get("startTime"),
                root.get("endTime")
        ).where(
                criteriaBuilder.equal(root.get("id"), id)
        );

        return getSession().createQuery(criteriaQuery)
                .setComment(
                        concatenate("GroupWorkout with id = ", id.toString())
                )
                .getResultList()
                .stream()
                .findFirst();
    }

    @Override
    public List<GroupWorkoutFullDto> getAllGroupWorkoutByGroupId(UUID groupId) {

        Assert.notNull(groupId, "groupId must not be null");

        CriteriaBuilder criteriaBuilder = getSession().getCriteriaBuilder();
        CriteriaQuery<GroupWorkoutFullDto> criteriaQuery = criteriaBuilder.createQuery(GroupWorkoutFullDto.class);
        Root<GroupWorkout> root = criteriaQuery.from(GroupWorkout.class);

        criteriaQuery.multiselect(
                root.get("id"),
                root.get("group.id"),
                root.get("workoutId"),
                root.get("startTime"),
                root.get("endTime")
        ).where(
                criteriaBuilder.equal(root.get("group.id"), groupId)
        );

        return getSession().createQuery(criteriaQuery)
                .setComment(
                        concatenate("GroupWorkout with groupId = ", groupId.toString())
                )
                .getResultList();
    }

    @Override
    public List<GroupWorkoutFullDto> getAllGroupWorkoutByWorkoutId(UUID workoutId) {

        Assert.notNull(workoutId, "workoutId must not be null");

        CriteriaBuilder criteriaBuilder = getSession().getCriteriaBuilder();
        CriteriaQuery<GroupWorkoutFullDto> criteriaQuery = criteriaBuilder.createQuery(GroupWorkoutFullDto.class);
        Root<GroupWorkout> root = criteriaQuery.from(GroupWorkout.class);

        criteriaQuery.multiselect(
                root.get("id"),
                root.get("group.id"),
                root.get("workoutId"),
                root.get("startTime"),
                root.get("endTime")
            ).where(
                criteriaBuilder.equal(root.get("workoutId"), workoutId)
            );

        return getSession().createQuery(criteriaQuery)
                .setComment(
                        concatenate("GroupWorkout with workoutId = ", workoutId.toString())
                )
                .getResultList();
    }

    @Override
    public List<GroupWorkoutFullDto> getAllGroupsWorkouts() {

        return getAll().stream()
                .map(groupWorkoutFullDtoFactory::create)
                .collect(Collectors.toList());
    }
}

