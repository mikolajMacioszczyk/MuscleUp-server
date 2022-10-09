package groups.group.repository;

import groups.common.abstracts.AbstractHibernateQuery;
import groups.group.entity.*;
import org.hibernate.SessionFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Primary;
import org.springframework.stereotype.Repository;
import org.springframework.util.Assert;

import java.util.List;
import java.util.Optional;
import java.util.stream.Collectors;

import static java.util.Objects.isNull;

@Primary
@Repository
public class GroupHibernateQuery extends AbstractHibernateQuery<Group> implements GroupQuery {

    private final GroupFullDtoFactory groupFullDtoFactory;
    private final GroupNameDtoFactory groupNameDtoFactory;


    @Autowired
    protected GroupHibernateQuery(SessionFactory sessionFactory) {

        super(Group.class, sessionFactory);

        this.groupFullDtoFactory = new GroupFullDtoFactory();
        this.groupNameDtoFactory = new GroupNameDtoFactory();
    }


    @Override
    public Optional<GroupFullDto> findGroupById(Long id) {

        Assert.notNull(id, "id must not be null");

        Group group = getById(id);

        return isNull(group)?
                Optional.empty() :
                Optional.of(groupFullDtoFactory.create(group));
    }

    @Override
    public List<GroupFullDto> getAllGroups() {

        return getAll().stream()
                .map(groupFullDtoFactory::create)
                .collect(Collectors.toList());
    }

    @Override
    public List<GroupNameDto> getAllGroupNames() {

        return getAll().stream()
                .map(groupNameDtoFactory::create)
                .collect(Collectors.toList());
    }
}
