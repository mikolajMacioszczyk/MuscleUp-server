package content.criterion.repository;

import content.common.abstracts.AbstractHibernateRepository;
import content.criterion.entity.Criterion;
import org.hibernate.SessionFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Primary;
import org.springframework.stereotype.Repository;
import org.springframework.util.Assert;

import javax.persistence.criteria.*;
import java.util.List;
import java.util.UUID;

@Primary
@Repository
public class CriterionHibernateRepository extends AbstractHibernateRepository<Criterion> implements CriterionRepository {


    @Autowired
    CriterionHibernateRepository(SessionFactory sessionFactory) {

        super(Criterion.class, sessionFactory);
    }


    @Override
    public List<Criterion> getByIds(List<UUID> ids) {

        Assert.notNull(ids, "ids must not be null");
        ids.forEach(id -> Assert.notNull(id, "id must not be null"));

        CriteriaBuilder criteriaBuilder = getSession().getCriteriaBuilder();
        CriteriaQuery<Criterion> criteriaQuery = criteriaBuilder.createQuery(Criterion.class);
        Root<Criterion> root = criteriaQuery.from(Criterion.class);

        Expression<UUID> idExpression = root.get("id");

        criteriaQuery.multiselect(
                root.get("id"),
                root.get("name"),
                root.get("unit"),
                root.get("active")
        ).where(
                idExpression.in(ids)
        );

        return getSession().createQuery(criteriaQuery)
                .setComment("Criteria with given ids with ids")
                .getResultList();
    }
}