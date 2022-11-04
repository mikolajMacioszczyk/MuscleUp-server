package groups.groupPermission.repository;

import groups.common.abstracts.AbstractHibernateRepository;
import groups.groupPermission.entity.GroupPermission;
import org.hibernate.SessionFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Primary;
import org.springframework.stereotype.Repository;
import org.springframework.util.Assert;

import javax.persistence.criteria.CriteriaBuilder;
import javax.persistence.criteria.CriteriaDelete;
import javax.persistence.criteria.Root;
import javax.transaction.Transactional;
import java.util.UUID;

import static groups.common.utils.StringUtils.concatenate;

@Primary
@Repository
public class GroupPermissionHibernateRepository extends AbstractHibernateRepository<GroupPermission> implements GroupPermissionRepository {


    @Autowired
    GroupPermissionHibernateRepository(SessionFactory sessionFactory) {

        super(GroupPermission.class, sessionFactory);
    }


    @Override
    @Transactional
    public UUID add(GroupPermission groupPermission) {

        Assert.notNull(groupPermission, "groupPermission must not be null");

        return save(groupPermission);
    }

    @Override
    @Transactional
    public void remove(UUID groupPermissionId) {

        Assert.notNull(groupPermissionId, "groupPermissionId must not be null");

        delete(groupPermissionId);
    }

    @Override
    @Transactional
    public void remove(UUID groupId, UUID permissionId) {

        Assert.notNull(groupId, "groupId must not be null");
        Assert.notNull(permissionId, "permissionId must not be null");

        CriteriaBuilder criteriaBuilder = getSession().getCriteriaBuilder();
        CriteriaDelete<GroupPermission> criteriaDelete = criteriaBuilder.createCriteriaDelete(GroupPermission.class);
        Root<GroupPermission> root = criteriaDelete.from(GroupPermission.class);

        criteriaDelete.where(
                criteriaBuilder.equal(root.get("permissionId"), permissionId),
                criteriaBuilder.equal(root.get("group").get("id"), groupId)
        );

        getSession().createQuery(criteriaDelete)
                .setComment(
                        concatenate("Delete GroupParticipant with GroupId = ",
                                groupId.toString(),
                                " and PermissionId = ",
                                permissionId.toString()
                        )
                )
                .executeUpdate();
    }
}

