package groups.groupPermission.repository;

import groups.common.abstracts.AbstractHibernateQuery;
import groups.groupPermission.entity.GroupPermission;
import groups.groupPermission.entity.GroupPermissionDto;
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
import java.util.UUID;

import static groups.common.utils.StringUtils.concatenate;

@Primary
@Repository
public class GroupPermissionHibernateQuery extends AbstractHibernateQuery<GroupPermission> implements GroupPermissionQuery {


    @Autowired
    GroupPermissionHibernateQuery(SessionFactory sessionFactory) {

        super(GroupPermission.class, sessionFactory);
    }


    @Override
    @Transactional
    public List<GroupPermissionDto> getAllGroupPermissionsByGroupIdAndPermissionId(UUID groupId, UUID permissionId) {

        Assert.notNull(groupId, "groupId must not be null");
        Assert.notNull(permissionId, "permissionId must not be null");

        CriteriaBuilder criteriaBuilder = getSession().getCriteriaBuilder();
        CriteriaQuery<GroupPermissionDto> criteriaQuery = criteriaBuilder.createQuery(GroupPermissionDto.class);
        Root<GroupPermission> root = criteriaQuery.from(GroupPermission.class);

        criteriaQuery.multiselect(
                root.get("group").get("id"),
                root.get("permissionId")
        ).where(
                criteriaBuilder.equal(root.get("group").get("id"), groupId),
                criteriaBuilder.equal(root.get("permissionId"), permissionId)
        );

        return getSession().createQuery(criteriaQuery)
                .setComment(
                        concatenate("GroupParticipant with groupId = ",
                                groupId.toString(),
                                " and permissionId =",
                                permissionId.toString()
                        )
                )
                .getResultList();
    }
}
