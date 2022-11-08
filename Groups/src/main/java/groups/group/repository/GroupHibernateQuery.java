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

    private final GroupDtoFactory groupDtoFactory;


    @Autowired
    GroupHibernateQuery(SessionFactory sessionFactory) {

        super(Group.class, sessionFactory);

        this.groupDtoFactory = new GroupDtoFactory();
    }


    @Override
    @Transactional
    public Optional<GroupDto> findGroupById(UUID id) {

        Assert.notNull(id, "id must not be null");

        CriteriaBuilder criteriaBuilder = getSession().getCriteriaBuilder();
        CriteriaQuery<GroupDto> criteriaQuery = criteriaBuilder.createQuery(GroupDto.class);
        Root<Group> root = criteriaQuery.from(Group.class);

        criteriaQuery.multiselect(
                root.get("id"),
                root.get("name"),
                root.get("trainerId"),
                root.get("fitnessClubId"),
                root.get("description"),
                root.get("location"),
                root.get("maxParticipants"),
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
    public List<GroupDto> getAllGroups() {

        return getAll().stream()
                .map(groupDtoFactory::create)
                .collect(Collectors.toList());
    }
}
