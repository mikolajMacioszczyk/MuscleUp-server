package content.bodyPart.repository;

import content.bodyPart.entity.BodyPart;
import content.common.abstracts.AbstractHibernateRepository;
import org.hibernate.SessionFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Primary;
import org.springframework.stereotype.Repository;

@Primary
@Repository
public class BodyPartHibernateRepository extends AbstractHibernateRepository<BodyPart> implements BodyPartRepository {

    @Autowired
    BodyPartHibernateRepository(SessionFactory sessionFactory) {

        super(BodyPart.class, sessionFactory);
    }
}