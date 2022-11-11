package content.criterion.repository;

import content.common.abstracts.AbstractHibernateQuery;
import content.criterion.entity.Criterion;
import content.criterion.entity.CriterionDto;
import content.criterion.entity.CriterionDtoFactory;
import org.hibernate.SessionFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Primary;
import org.springframework.stereotype.Repository;
import org.springframework.util.Assert;

import javax.transaction.Transactional;
import java.util.List;
import java.util.Optional;
import java.util.UUID;

import static java.util.Objects.isNull;
import static java.util.Optional.empty;
import static java.util.Optional.of;


@Primary
@Repository
public class CriterionHibernateQuery extends AbstractHibernateQuery<Criterion> implements CriterionQuery {

    private final CriterionDtoFactory criterionDtoFactory;


    @Autowired
    CriterionHibernateQuery(SessionFactory sessionFactory) {

        super(Criterion.class, sessionFactory);

        this.criterionDtoFactory = new CriterionDtoFactory();
    }


    @Override
    public CriterionDto get(UUID id) {

        return criterionDtoFactory.create(getById(id));
    }

    @Override
    @Transactional
    public Optional<CriterionDto> findById(UUID id) {

        Assert.notNull(id, "id must not be null");

        Criterion criterion = getById(id);

        return isNull(criterion)? empty() : of(criterionDtoFactory.create(criterion));
    }

    @Override
    public List<CriterionDto> getAllCriteria() {

        return getAll().stream()
                .map(criterionDtoFactory::create)
                .toList();
    }

    @Override
    public List<CriterionDto> getAllActiveCriteria() {

        return getAll().stream()
                .filter(Criterion::isActive)
                .map(criterionDtoFactory::create)
                .toList();
    }
}
