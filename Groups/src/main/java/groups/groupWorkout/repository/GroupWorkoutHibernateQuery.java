package groups.groupWorkout.repository;

import groups.common.abstracts.AbstractHibernateQuery;
import groups.common.wrappers.TimeWrapper;
import groups.common.wrappers.UuidWrapper;
import groups.groupWorkout.entity.GroupWorkout;
import groups.groupWorkout.entity.GroupWorkoutDto;
import groups.groupWorkout.entity.GroupWorkoutDtoFactory;
import org.hibernate.SessionFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Primary;
import org.springframework.stereotype.Repository;
import org.springframework.util.Assert;

import javax.persistence.criteria.CriteriaBuilder;
import javax.persistence.criteria.CriteriaQuery;
import javax.persistence.criteria.Root;
import javax.transaction.Transactional;
import java.time.ZonedDateTime;
import java.util.List;
import java.util.Optional;
import java.util.UUID;
import java.util.stream.Collectors;

import static groups.common.utils.StringUtils.concatenate;

@Primary
@Repository
public class GroupWorkoutHibernateQuery extends AbstractHibernateQuery<GroupWorkout> implements GroupWorkoutQuery {

    private final GroupWorkoutDtoFactory groupWorkoutDtoFactory;


    @Autowired
    GroupWorkoutHibernateQuery(SessionFactory sessionFactory) {

        super(GroupWorkout.class, sessionFactory);

        this.groupWorkoutDtoFactory = new GroupWorkoutDtoFactory();
    }


    @Override
    public TimeWrapper getTimeById(UUID id) {

        Assert.notNull(id, "id must not be null");

        CriteriaBuilder criteriaBuilder = getSession().getCriteriaBuilder();
        CriteriaQuery<TimeWrapper> criteriaQuery = criteriaBuilder.createQuery(TimeWrapper.class);
        Root<GroupWorkout> root = criteriaQuery.from(GroupWorkout.class);

        criteriaQuery.multiselect(
                root.get("startTime"),
                root.get("endTime")
        ).where(
                criteriaBuilder.equal(root.get("id"), id)
        );

        return getSession().createQuery(criteriaQuery)
                .setComment(
                        concatenate("Time based on id = ", id.toString())
                )
                .getSingleResult();
    }

    @Override
    @Transactional
    public Optional<GroupWorkoutDto> findGroupWorkoutById(UUID id) {

        Assert.notNull(id, "id must not be null");

        CriteriaBuilder criteriaBuilder = getSession().getCriteriaBuilder();
        CriteriaQuery<GroupWorkoutDto> criteriaQuery = criteriaBuilder.createQuery(GroupWorkoutDto.class);
        Root<GroupWorkout> root = criteriaQuery.from(GroupWorkout.class);

        criteriaQuery.multiselect(
                root.get("id"),
                root.get("group").get("id"),
                root.get("workoutId"),
                root.get("startTime"),
                root.get("endTime"),
                root.get("cloneId")
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
    public List<GroupWorkoutDto> getAllGroupsWorkouts() {

        return getAll().stream()
                .map(groupWorkoutDtoFactory::create)
                .collect(Collectors.toList());
    }

    @Override
    public UUID getCloneIdById(UUID id) {

        Assert.notNull(id, "id must not be null");

        CriteriaBuilder criteriaBuilder = getSession().getCriteriaBuilder();
        CriteriaQuery<UuidWrapper> criteriaQuery = criteriaBuilder.createQuery(UuidWrapper.class);
        Root<GroupWorkout> root = criteriaQuery.from(GroupWorkout.class);

        criteriaQuery.multiselect(
                root.get("cloneId")
        ).where(
                criteriaBuilder.equal(root.get("id"), id)
        );

        return getSession().createQuery(criteriaQuery)
                .setComment(
                        concatenate("GroupWorkout cloneId by id = ", id.toString())
                )
                .getSingleResult()
                .uuid();
    }

    @Override
    public List<UUID> getFutureGroupWorkoutsByCloneId(UUID id) {

        Assert.notNull(id, "id must not be null");

        CriteriaBuilder criteriaBuilder = getSession().getCriteriaBuilder();
        CriteriaQuery<UuidWrapper> criteriaQuery = criteriaBuilder.createQuery(UuidWrapper.class);
        Root<GroupWorkout> root = criteriaQuery.from(GroupWorkout.class);

        criteriaQuery.multiselect(
                root.get("id")
        ).where(
                criteriaBuilder.equal(root.get("cloneId"), id),
                criteriaBuilder.greaterThan(root.get("startTime"), ZonedDateTime.now())
        );

        return getSession().createQuery(criteriaQuery)
                .setComment(
                        concatenate("GroupWorkout ids by cloneId = ", id.toString())
                )
                .getResultList()
                .stream()
                .map(UuidWrapper::uuid)
                .toList();
    }

    @Override
    public UUID getFitnessClubIdByGroupWorkoutId(UUID id) {

        Assert.notNull(id, "id must not be null");

        CriteriaBuilder criteriaBuilder = getSession().getCriteriaBuilder();
        CriteriaQuery<UuidWrapper> criteriaQuery = criteriaBuilder.createQuery(UuidWrapper.class);
        Root<GroupWorkout> root = criteriaQuery.from(GroupWorkout.class);

        criteriaQuery.multiselect(
                root.get("group").get("fitnessClubId")
        ).where(
                criteriaBuilder.equal(root.get("id"), id)
        );

        return getSession().createQuery(criteriaQuery)
                .setComment(
                        concatenate("FitnessClubId by GroupWorkoutId = ", id.toString())
                )
                .getSingleResult()
                .uuid();
    }
}



