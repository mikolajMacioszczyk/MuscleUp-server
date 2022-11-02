package groups.groupWorkout.repository;

import groups.common.abstracts.AbstractHibernateQuery;
import groups.groupWorkout.entity.GroupWorkout;
import groups.groupWorkout.entity.GroupWorkoutFullDto;
import groups.groupWorkout.entity.GroupWorkoutFullDtoFactory;
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

import static groups.common.utils.StringUtils.concatenate;

@Primary
@Repository
public class GroupWorkoutHibernateQuery extends AbstractHibernateQuery<GroupWorkout> implements GroupWorkoutQuery {

    private final GroupWorkoutFullDtoFactory groupWorkoutFullDtoFactory;


    @Autowired
    GroupWorkoutHibernateQuery(SessionFactory sessionFactory) {

        super(GroupWorkout.class, sessionFactory);

        this.groupWorkoutFullDtoFactory = new GroupWorkoutFullDtoFactory();
    }


    @Override
    @Transactional
    public Optional<GroupWorkoutFullDto> findGroupWorkoutById(UUID id) {

        Assert.notNull(id, "id must not be null");

        CriteriaBuilder criteriaBuilder = getSession().getCriteriaBuilder();
        CriteriaQuery<GroupWorkoutFullDto> criteriaQuery = criteriaBuilder.createQuery(GroupWorkoutFullDto.class);
        Root<GroupWorkout> root = criteriaQuery.from(GroupWorkout.class);

        criteriaQuery.multiselect(
                root.get("id"),
                root.get("group").get("id"),
                root.get("location"),
                root.get("maxParticipants")
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
    @Transactional
    public List<GroupWorkoutFullDto> getAllGroupWorkoutByGroupId(UUID groupId) {

        Assert.notNull(groupId, "groupId must not be null");

        CriteriaBuilder criteriaBuilder = getSession().getCriteriaBuilder();
        CriteriaQuery<GroupWorkoutFullDto> criteriaQuery = criteriaBuilder.createQuery(GroupWorkoutFullDto.class);
        Root<GroupWorkout> root = criteriaQuery.from(GroupWorkout.class);

        criteriaQuery.multiselect(
                root.get("id"),
                root.get("group").get("id"),
                root.get("workoutId"),
                root.get("location"),
                root.get("maxParticipants")
        ).where(
                criteriaBuilder.equal(root.get("group").get("id"), groupId)
        );

        return getSession().createQuery(criteriaQuery)
                .setComment(
                        concatenate("GroupWorkout with groupId = ", groupId.toString())
                )
                .getResultList();
    }

    @Override
    @Transactional
    public List<GroupWorkoutFullDto> getAllGroupWorkoutByWorkoutId(UUID workoutId) {

        Assert.notNull(workoutId, "workoutId must not be null");

        CriteriaBuilder criteriaBuilder = getSession().getCriteriaBuilder();
        CriteriaQuery<GroupWorkoutFullDto> criteriaQuery = criteriaBuilder.createQuery(GroupWorkoutFullDto.class);
        Root<GroupWorkout> root = criteriaQuery.from(GroupWorkout.class);

        criteriaQuery.multiselect(
                root.get("id"),
                root.get("group").get("id"),
                root.get("workoutId"),
                root.get("location"),
                root.get("maxParticipants")
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
