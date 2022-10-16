package groups.workoutPermission.repository;

import groups.common.abstracts.AbstractHibernateQuery;
import groups.workoutPermission.entity.WorkoutPermission;
import groups.workoutPermission.entity.WorkoutPermissionFullDto;
import groups.workoutPermission.entity.WorkoutPermissionFullDtoFactory;
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
public class WorkoutPermissionHibernateQuery extends AbstractHibernateQuery<WorkoutPermission> implements WorkoutPermissionQuery {

    private final WorkoutPermissionFullDtoFactory workoutPermissionFullDtoFactory;


    @Autowired
    WorkoutPermissionHibernateQuery(SessionFactory sessionFactory) {

        super(WorkoutPermission.class, sessionFactory);

        this.workoutPermissionFullDtoFactory = new WorkoutPermissionFullDtoFactory();
    }


    @Override
    public List<WorkoutPermissionFullDto> getAllWorkoutPermissions() {

        return getAll().stream()
                .map(workoutPermissionFullDtoFactory::create)
                .collect(Collectors.toList());
    }

    @Override
    public Optional<WorkoutPermissionFullDto> findWorkoutPermissionById(UUID id) {

        Assert.notNull(id, "id must not be null");

        CriteriaBuilder criteriaBuilder = getSession().getCriteriaBuilder();
        CriteriaQuery<WorkoutPermissionFullDto> criteriaQuery = criteriaBuilder.createQuery(WorkoutPermissionFullDto.class);
        Root<WorkoutPermission> root = criteriaQuery.from(WorkoutPermission.class);

        criteriaQuery.multiselect(
                root.get("id"),
                root.get("groupWorkout").get("id"),
                root.get("permissionId")
        ).where(
                criteriaBuilder.equal(root.get("id"), id)
        );

        return getSession().createQuery(criteriaQuery)
                .setComment(
                        concatenate("WorkoutParticipant with id = ", id.toString())
                )
                .getResultList()
                .stream()
                .findFirst();
    }

    @Override
    public List<WorkoutPermissionFullDto> getAllWorkoutPermissionsByGroupWorkoutId(UUID groupWorkoutId) {

        Assert.notNull(groupWorkoutId, "groupWorkoutId must not be null");

        CriteriaBuilder criteriaBuilder = getSession().getCriteriaBuilder();
        CriteriaQuery<WorkoutPermissionFullDto> criteriaQuery = criteriaBuilder.createQuery(WorkoutPermissionFullDto.class);
        Root<WorkoutPermission> root = criteriaQuery.from(WorkoutPermission.class);

        criteriaQuery.multiselect(
                root.get("id"),
                root.get("groupWorkout").get("id"),
                root.get("permissionId")
        ).where(
                criteriaBuilder.equal(root.get("groupWorkout").get("id"), groupWorkoutId)
        );

        return getSession().createQuery(criteriaQuery)
                .setComment(
                        concatenate("WorkoutParticipant with groupWorkoutId =", groupWorkoutId.toString())
                )
                .getResultList();
    }

    @Override
    public List<WorkoutPermissionFullDto> getAllWorkoutPermissionsByPermissionId(UUID permissionId) {

        Assert.notNull(permissionId, "permissionId must not be null");

        CriteriaBuilder criteriaBuilder = getSession().getCriteriaBuilder();
        CriteriaQuery<WorkoutPermissionFullDto> criteriaQuery = criteriaBuilder.createQuery(WorkoutPermissionFullDto.class);
        Root<WorkoutPermission> root = criteriaQuery.from(WorkoutPermission.class);

        criteriaQuery.multiselect(
                root.get("id"),
                root.get("groupWorkout").get("id"),
                root.get("permissionId")
        ).where(
                criteriaBuilder.equal(root.get("permissionId"), permissionId)
        );

        return getSession().createQuery(criteriaQuery)
                .setComment(
                        concatenate("WorkoutParticipant with permissionId =", permissionId.toString())
                )
                .getResultList();
    }

    @Override
    public List<WorkoutPermissionFullDto> getAllWorkoutPermissionsByGroupWorkoutIdAndPermissionId(UUID groupWorkoutId, UUID permissionId) {

        Assert.notNull(groupWorkoutId, "groupWorkoutId must not be null");
        Assert.notNull(permissionId, "permissionId must not be null");

        CriteriaBuilder criteriaBuilder = getSession().getCriteriaBuilder();
        CriteriaQuery<WorkoutPermissionFullDto> criteriaQuery = criteriaBuilder.createQuery(WorkoutPermissionFullDto.class);
        Root<WorkoutPermission> root = criteriaQuery.from(WorkoutPermission.class);

        criteriaQuery.multiselect(
                root.get("id"),
                root.get("groupWorkout").get("id"),
                root.get("permissionId")
        ).where(
                criteriaBuilder.equal(root.get("groupWorkout").get("id"), groupWorkoutId),
                criteriaBuilder.equal(root.get("permissionId"), permissionId)
        );

        return getSession().createQuery(criteriaQuery)
                .setComment(
                        concatenate("WorkoutParticipant with groupWorkoutId = ",
                                groupWorkoutId.toString(),
                                " and permissionId =",
                                permissionId.toString()
                        )
                )
                .getResultList();
    }
}
