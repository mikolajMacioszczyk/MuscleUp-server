package groups.workoutPermission.repository;

import groups.common.abstracts.AbstractHibernateRepository;
import groups.workoutParticipant.entity.WorkoutParticipant;
import groups.workoutPermission.entity.WorkoutPermission;
import org.hibernate.SessionFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Primary;
import org.springframework.stereotype.Repository;
import org.springframework.util.Assert;

import javax.persistence.criteria.CriteriaBuilder;
import javax.persistence.criteria.CriteriaDelete;
import javax.persistence.criteria.Root;
import java.util.UUID;

import static groups.common.stringUtils.StringUtils.concatenate;

@Primary
@Repository
public class WorkoutPermissionHibernateRepository extends AbstractHibernateRepository<WorkoutPermission> implements WorkoutPermissionRepository {


    @Autowired
    private WorkoutPermissionHibernateRepository(SessionFactory sessionFactory) {

        super(WorkoutPermission.class, sessionFactory);
    }


    @Override
    public UUID add(WorkoutPermission workoutPermission) {

        Assert.notNull(workoutPermission, "workoutPermission must not be null");

        return save(workoutPermission);
    }

    @Override
    public void remove(UUID workoutPermissionId) {

        Assert.notNull(workoutPermissionId, "workoutPermissionId must not be null");

        delete(workoutPermissionId);
    }

    @Override
    public void remove(UUID groupWorkoutId, UUID permissionId) {

        Assert.notNull(groupWorkoutId, "groupWorkoutId must not be null");
        Assert.notNull(permissionId, "permissionId must not be null");

        CriteriaBuilder criteriaBuilder = getSession().getCriteriaBuilder();
        CriteriaDelete<WorkoutPermission> criteriaDelete = criteriaBuilder.createCriteriaDelete(WorkoutPermission.class);
        Root<WorkoutPermission> root = criteriaDelete.from(WorkoutPermission.class);

        criteriaDelete.where(
                criteriaBuilder.equal(root.get("permissionId"), permissionId),
                criteriaBuilder.equal(root.get("groupWorkout.id"), groupWorkoutId)
        );

        getSession().createQuery(criteriaDelete)
                .setComment(
                        concatenate("Delete WorkoutParticipant with GroupWorkoutId = ",
                                groupWorkoutId.toString(),
                                " and PermissionId = ",
                                permissionId.toString()
                        )
                )
                .executeUpdate();
    }
}

