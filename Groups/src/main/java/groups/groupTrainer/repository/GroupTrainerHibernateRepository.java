package groups.groupTrainer.repository;

import groups.common.abstracts.AbstractHibernateRepository;
import groups.groupTrainer.entity.GroupTrainer;
import org.hibernate.SessionFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Primary;
import org.springframework.stereotype.Repository;

import java.util.UUID;

@Primary
@Repository
public class GroupTrainerHibernateRepository extends AbstractHibernateRepository<GroupTrainer> implements GroupTrainerRepository {

    @Autowired
    protected GroupTrainerHibernateRepository(SessionFactory sessionFactory) {

        super(GroupTrainer.class, sessionFactory);
    }

    @Override
    public UUID assign(GroupTrainer groupTrainer) {

        return save(groupTrainer);
    }
}
