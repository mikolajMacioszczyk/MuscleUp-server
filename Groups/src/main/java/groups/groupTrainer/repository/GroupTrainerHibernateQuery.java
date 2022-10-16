package groups.groupTrainer.repository;

import groups.common.abstracts.AbstractHibernateQuery;
import groups.groupTrainer.entity.GroupTrainer;
import groups.groupTrainer.entity.GroupTrainerFullDto;
import groups.groupTrainer.entity.GroupTrainerFullDtoFactory;
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
public class GroupTrainerHibernateQuery extends AbstractHibernateQuery<GroupTrainer> implements GroupTrainerQuery {

    private final GroupTrainerFullDtoFactory groupTrainerFullDtoFactory;


    @Autowired
    private GroupTrainerHibernateQuery(SessionFactory sessionFactory) {

        super(GroupTrainer.class, sessionFactory);

        this.groupTrainerFullDtoFactory = new GroupTrainerFullDtoFactory();
    }


    @Override
    public List<GroupTrainerFullDto> getAllGroupsTrainers() {

        return getAll().stream()
                .map(groupTrainerFullDtoFactory::create)
                .collect(Collectors.toList());
    }

    @Override
    public Optional<GroupTrainerFullDto> findGroupTrainerById(UUID id) {

        Assert.notNull(id, "id must not be null");

        CriteriaBuilder criteriaBuilder = getSession().getCriteriaBuilder();
        CriteriaQuery<GroupTrainerFullDto> criteriaQuery = criteriaBuilder.createQuery(GroupTrainerFullDto.class);
        Root<GroupTrainer> root = criteriaQuery.from(GroupTrainer.class);

        criteriaQuery.multiselect(
                root.get("id"),
                root.get("group.id"),
                root.get("trainerId")
        ).where(
                criteriaBuilder.equal(root.get("id"), id)
        );

        return getSession().createQuery(criteriaQuery)
                .setComment(
                        concatenate("GroupTrainer with id = ", id.toString())
                )
                .getResultList()
                .stream()
                .findFirst();
    }

    @Override
    public List<GroupTrainerFullDto> getAllGroupTrainerByGroupId(UUID groupId) {

        Assert.notNull(groupId, "groupId must not be null");

        CriteriaBuilder criteriaBuilder = getSession().getCriteriaBuilder();
        CriteriaQuery<GroupTrainerFullDto> criteriaQuery = criteriaBuilder.createQuery(GroupTrainerFullDto.class);
        Root<GroupTrainer> root = criteriaQuery.from(GroupTrainer.class);

        criteriaQuery.multiselect(
                root.get("id"),
                root.get("group.id"),
                root.get("trainerId")
        ).where(
                criteriaBuilder.equal(root.get("group.id"), groupId)
        );

        return getSession().createQuery(criteriaQuery)
                .setComment(
                        concatenate("GroupTrainer with groupId = ", groupId.toString())
                )
                .getResultList();
    }

    @Override
    public List<GroupTrainerFullDto> getAllGroupTrainerByTrainerId(UUID trainerId) {

        Assert.notNull(trainerId, "trainerId must not be null");

        CriteriaBuilder criteriaBuilder = getSession().getCriteriaBuilder();
        CriteriaQuery<GroupTrainerFullDto> criteriaQuery = criteriaBuilder.createQuery(GroupTrainerFullDto.class);
        Root<GroupTrainer> root = criteriaQuery.from(GroupTrainer.class);

        criteriaQuery.multiselect(
                root.get("id"),
                root.get("group.id"),
                root.get("trainerId")
        ).where(
                criteriaBuilder.equal(root.get("trainerId"), trainerId)
        );

        return getSession().createQuery(criteriaQuery)
                .setComment(
                        concatenate("GroupTrainer with trainerId = ", trainerId.toString())
                )
                .getResultList();
    }

    @Override
    public List<GroupTrainerFullDto> getAllGroupTrainerByGroupIdAndTrainerId(UUID groupId, UUID trainerId) {

        Assert.notNull(groupId, "groupId must not be null");
        Assert.notNull(trainerId, "trainerId must not be null");

        CriteriaBuilder criteriaBuilder = getSession().getCriteriaBuilder();
        CriteriaQuery<GroupTrainerFullDto> criteriaQuery = criteriaBuilder.createQuery(GroupTrainerFullDto.class);
        Root<GroupTrainer> root = criteriaQuery.from(GroupTrainer.class);

        criteriaQuery.multiselect(
                root.get("id"),
                root.get("group.id"),
                root.get("trainerId")
        ).where(
                criteriaBuilder.equal(root.get("trainerId"), trainerId),
                criteriaBuilder.equal(root.get("group.id"), groupId)
        );

        return getSession().createQuery(criteriaQuery)
                .setComment(
                        concatenate("GroupTrainer with trainerId = ",
                                trainerId.toString(),
                                " and groupId =",
                                groupId.toString()
                        )
                )
                .getResultList();
    }
}