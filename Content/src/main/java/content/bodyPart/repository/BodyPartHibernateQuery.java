package content.bodyPart.repository;

import content.bodyPart.entity.BodyPart;
import content.bodyPart.entity.BodyPartDto;
import content.bodyPart.entity.BodyPartDtoFactory;
import content.common.abstracts.AbstractHibernateQuery;
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


@Primary
@Repository
public class BodyPartHibernateQuery extends AbstractHibernateQuery<BodyPart> implements BodyPartQuery {

    private final BodyPartDtoFactory bodyPartDtoFactory;


    @Autowired
    BodyPartHibernateQuery(SessionFactory sessionFactory) {

        super(BodyPart.class, sessionFactory);

        this.bodyPartDtoFactory = new BodyPartDtoFactory();
    }


    @Override
    @Transactional
    public Optional<BodyPartDto> findById(UUID id) {

        Assert.notNull(id, "id must not be null");

        CriteriaBuilder criteriaBuilder = getSession().getCriteriaBuilder();
        CriteriaQuery<BodyPartDto> criteriaQuery = criteriaBuilder.createQuery(BodyPartDto.class);
        Root<BodyPart> root = criteriaQuery.from(BodyPart.class);

        criteriaQuery.multiselect(
                root.get("id"),
                root.get("name")
        ).where(
                criteriaBuilder.equal(root.get("id"), id)
        );

        return getSession().createQuery(criteriaQuery)
                .setComment("BodyPart with id = " +  id)
                .getResultList()
                .stream()
                .findFirst();
    }

    @Override
    public List<BodyPartDto> getAllBodyParts() {

        return getAll().stream()
                .map(bodyPartDtoFactory::create)
                .collect(Collectors.toList());
    }
}
