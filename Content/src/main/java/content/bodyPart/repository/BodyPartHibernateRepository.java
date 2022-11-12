package content.bodyPart.repository;

import content.bodyPart.entity.BodyPart;
import content.common.abstracts.AbstractHibernateRepository;
import org.hibernate.SessionFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Primary;
import org.springframework.stereotype.Repository;
import org.springframework.util.Assert;

import javax.persistence.criteria.CriteriaBuilder;
import javax.persistence.criteria.CriteriaQuery;
import javax.persistence.criteria.Expression;
import javax.persistence.criteria.Root;
import java.util.List;
import java.util.UUID;

@Primary
@Repository
public class BodyPartHibernateRepository extends AbstractHibernateRepository<BodyPart> implements BodyPartRepository {

    @Autowired
    BodyPartHibernateRepository(SessionFactory sessionFactory) {

        super(BodyPart.class, sessionFactory);
    }


    @Override
    public List<BodyPart> getByIds(List<UUID> ids) {

        Assert.notNull(ids, "ids must not be null");
        ids.forEach(id -> Assert.notNull(id, "id must not be null"));

        CriteriaBuilder criteriaBuilder = getSession().getCriteriaBuilder();
        CriteriaQuery<BodyPart> criteriaQuery = criteriaBuilder.createQuery(BodyPart.class);
        Root<BodyPart> root = criteriaQuery.from(BodyPart.class);

        Expression<UUID> idExpression = root.get("id");

        criteriaQuery.multiselect(
                root.get("id"),
                root.get("name")
        ).where(
                idExpression.in(ids)
        );

        return getSession().createQuery(criteriaQuery)
                .setComment("BodyParts with given ids")
                .getResultList();
    }
}