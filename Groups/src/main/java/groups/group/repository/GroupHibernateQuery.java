package groups.group.repository;

import groups.common.abstracts.AbstractHibernateQuery;
import groups.group.entity.*;
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
public class GroupHibernateQuery extends AbstractHibernateQuery<Group> implements GroupQuery {

    private final GroupFullDtoFactory groupFullDtoFactory;


    @Autowired
    GroupHibernateQuery(SessionFactory sessionFactory) {

        super(Group.class, sessionFactory);

        this.groupFullDtoFactory = new GroupFullDtoFactory();
    }


    @Override
    @Transactional
    public Optional<GroupFullDto> findGroupById(UUID id) {

        Assert.notNull(id, "id must not be null");

        CriteriaBuilder criteriaBuilder = getSession().getCriteriaBuilder();
        CriteriaQuery<GroupFullDto> criteriaQuery = criteriaBuilder.createQuery(GroupFullDto.class);
        Root<Group> root = criteriaQuery.from(Group.class);

        criteriaQuery.multiselect(
                root.get("id"),
                root.get("name"),
                root.get("description"),
                root.get("startTime"),
                root.get("endTime"),
                root.get("repeatable")
        ).where(
                criteriaBuilder.equal(root.get("id"), id)
        );

        return getSession().createQuery(criteriaQuery)
                .setComment(
                        concatenate("Group with id = ", id.toString())
                )
                .getResultList()
                .stream()
                .findFirst();
    }

    @Override
    public List<GroupFullDto> getAllGroups() {

        return getAll().stream()
                .map(groupFullDtoFactory::create)
                .collect(Collectors.toList());
    }
}
