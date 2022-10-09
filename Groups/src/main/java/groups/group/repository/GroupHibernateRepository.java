package groups.group.repository;

import groups.common.abstracts.AbstractHibernateRepository;
import groups.group.entity.Group;
import org.hibernate.SessionFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Primary;
import org.springframework.stereotype.Repository;


@Primary
@Repository
public class GroupHibernateRepository extends AbstractHibernateRepository<Group> implements GroupRepository {

    @Autowired
    protected GroupHibernateRepository(SessionFactory sessionFactory) {

        super(Group.class, sessionFactory);
    }
}
