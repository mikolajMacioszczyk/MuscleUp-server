package groups.workoutParticipant.repository;

import groups.common.abstracts.AbstractHibernateQuery;
import groups.workoutParticipant.entity.WorkoutParticipant;
import groups.workoutParticipant.entity.WorkoutParticipantFullDto;
import groups.workoutParticipant.entity.WorkoutParticipantFullDtoFactory;
import org.hibernate.SessionFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.util.Assert;

import javax.persistence.criteria.CriteriaBuilder;
import javax.persistence.criteria.CriteriaQuery;
import javax.persistence.criteria.Root;
import java.util.List;
import java.util.Optional;
import java.util.UUID;
import java.util.stream.Collectors;

import static groups.common.stringUtils.StringUtils.concatenate;

public class WorkoutParticipantHibernateQuery extends AbstractHibernateQuery<WorkoutParticipant> implements WorkoutParticipantQuery {

    private final WorkoutParticipantFullDtoFactory workoutParticipantFullDtoFactory;


    @Autowired
    protected WorkoutParticipantHibernateQuery(SessionFactory sessionFactory) {

        super(WorkoutParticipant.class, sessionFactory);

        this.workoutParticipantFullDtoFactory = new WorkoutParticipantFullDtoFactory();
    }


    @Override
    public List<WorkoutParticipantFullDto> getAllWorkoutParticipants() {

        return getAll().stream()
                .map(workoutParticipantFullDtoFactory::create)
                .collect(Collectors.toList());
    }

    @Override
    public Optional<WorkoutParticipantFullDto> findWorkoutParticipantById(UUID id) {

        Assert.notNull(id, "id must not be null");

        CriteriaBuilder criteriaBuilder = getSession().getCriteriaBuilder();
        CriteriaQuery<WorkoutParticipantFullDto> criteriaQuery = criteriaBuilder.createQuery(WorkoutParticipantFullDto.class);
        Root<WorkoutParticipant> root = criteriaQuery.from(WorkoutParticipant.class);

        criteriaQuery.multiselect(
                root.get("id"),
                root.get("groupWorkout.id"),
                root.get("gympassId")
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
    public List<WorkoutParticipantFullDto> getAllWorkoutParticipantsByGroupWorkoutId(UUID groupWorkoutId) {

        Assert.notNull(groupWorkoutId, "groupWorkoutId must not be null");

        CriteriaBuilder criteriaBuilder = getSession().getCriteriaBuilder();
        CriteriaQuery<WorkoutParticipantFullDto> criteriaQuery = criteriaBuilder.createQuery(WorkoutParticipantFullDto.class);
        Root<WorkoutParticipant> root = criteriaQuery.from(WorkoutParticipant.class);

        criteriaQuery.multiselect(
                root.get("id"),
                root.get("groupWorkout.id"),
                root.get("gympassId")
        ).where(
                criteriaBuilder.equal(root.get("groupWorkout.id"), groupWorkoutId)
        );

        return getSession().createQuery(criteriaQuery)
                .setComment(
                        concatenate("WorkoutParticipant with groupWorkoutId =", groupWorkoutId.toString())
                )
                .getResultList();
    }

    @Override
    public List<WorkoutParticipantFullDto> getAllWorkoutParticipantsByParticipantId(UUID participantId) {

        Assert.notNull(participantId, "participantId must not be null");

        CriteriaBuilder criteriaBuilder = getSession().getCriteriaBuilder();
        CriteriaQuery<WorkoutParticipantFullDto> criteriaQuery = criteriaBuilder.createQuery(WorkoutParticipantFullDto.class);
        Root<WorkoutParticipant> root = criteriaQuery.from(WorkoutParticipant.class);

        criteriaQuery.multiselect(
                root.get("id"),
                root.get("groupWorkout.id"),
                root.get("gympassId")
        ).where(
                criteriaBuilder.equal(root.get("gympassId"), participantId)
        );

        return getSession().createQuery(criteriaQuery)
                .setComment(
                        concatenate("WorkoutParticipant with participantId =", participantId.toString())
                )
                .getResultList();
    }

    @Override
    public List<WorkoutParticipantFullDto> getAllWorkoutParticipantsByGroupWorkoutIdAndParticipantId(UUID groupWorkoutId, UUID participantId) {

        Assert.notNull(participantId, "participantId must not be null");

        CriteriaBuilder criteriaBuilder = getSession().getCriteriaBuilder();
        CriteriaQuery<WorkoutParticipantFullDto> criteriaQuery = criteriaBuilder.createQuery(WorkoutParticipantFullDto.class);
        Root<WorkoutParticipant> root = criteriaQuery.from(WorkoutParticipant.class);

        criteriaQuery.multiselect(
                root.get("id"),
                root.get("groupWorkout.id"),
                root.get("gympassId")
        ).where(
                criteriaBuilder.equal(root.get("groupWorkout.id"), groupWorkoutId),
                criteriaBuilder.equal(root.get("gympassId"), participantId)
        );

        return getSession().createQuery(criteriaQuery)
                .setComment(
                        concatenate("WorkoutParticipant with groupWorkoutId = ",
                                groupWorkoutId.toString(),
                                " and participantId =",
                                participantId.toString()
                        )
                )
                .getResultList();
    }
}
