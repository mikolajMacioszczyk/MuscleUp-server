package groups.group.repository;

import groups.common.abstracts.AbstractHibernateQuery;
import groups.group.entity.*;
import org.hibernate.SessionFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Primary;
import org.springframework.stereotype.Repository;

import java.util.List;
import java.util.stream.Collectors;

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
