package groups.groupPermission.repository;

import groups.common.abstracts.AbstractHibernateQuery;
import groups.groupPermission.entity.GroupPermission;
import groups.groupPermission.entity.GroupPermissionFullDto;
import groups.groupPermission.entity.GroupPermissionFullDtoFactory;
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

import static groups.common.utils.StringUtils.concatenate;

@Primary
@Repository
public class GroupPermissionHibernateQuery extends AbstractHibernateQuery<GroupPermission> implements GroupPermissionQuery {

    private final GroupPermissionFullDtoFactory groupPermissionFullDtoFactory;


    @Autowired
    GroupPermissionHibernateQuery(SessionFactory sessionFactory) {

        super(GroupPermission.class, sessionFactory);

        this.groupPermissionFullDtoFactory = new GroupPermissionFullDtoFactory();
    }


    @Override
    public List<GroupPermissionFullDto> getAllGroupPermissions() {

        return getAll().stream()
                .map(groupPermissionFullDtoFactory::create)
                .collect(Collectors.toList());
    }

    @Override
    @Transactional
    public Optional<GroupPermissionFullDto> findGroupPermissionById(UUID id) {

        Assert.notNull(id, "id must not be null");

        CriteriaBuilder criteriaBuilder = getSession().getCriteriaBuilder();
        CriteriaQuery<GroupPermissionFullDto> criteriaQuery = criteriaBuilder.createQuery(GroupPermissionFullDto.class);
        Root<GroupPermission> root = criteriaQuery.from(GroupPermission.class);

        criteriaQuery.multiselect(
                root.get("id"),
                root.get("group").get("id"),
                root.get("permissionId")
        ).where(
                criteriaBuilder.equal(root.get("id"), id)
        );

        return getSession().createQuery(criteriaQuery)
                .setComment(
                        concatenate("GroupPermission with id = ", id.toString())
                )
                .getResultList()
                .stream()
                .findFirst();
    }

    @Override
    @Transactional
    public List<GroupPermissionFullDto> getAllGroupPermissionsByGroupId(UUID groupId) {

        Assert.notNull(groupId, "groupId must not be null");

        CriteriaBuilder criteriaBuilder = getSession().getCriteriaBuilder();
        CriteriaQuery<GroupPermissionFullDto> criteriaQuery = criteriaBuilder.createQuery(GroupPermissionFullDto.class);
        Root<GroupPermission> root = criteriaQuery.from(GroupPermission.class);

        criteriaQuery.multiselect(
                root.get("id"),
                root.get("group").get("id"),
                root.get("permissionId")
        ).where(
                criteriaBuilder.equal(root.get("group").get("id"), groupId)
        );

        return getSession().createQuery(criteriaQuery)
                .setComment(
                        concatenate("GroupPermission with groupId =", groupId.toString())
                )
                .getResultList();
    }

    @Override
    @Transactional
    public List<GroupPermissionFullDto> getAllGroupPermissionsByPermissionId(UUID permissionId) {

        Assert.notNull(permissionId, "permissionId must not be null");

        CriteriaBuilder criteriaBuilder = getSession().getCriteriaBuilder();
        CriteriaQuery<GroupPermissionFullDto> criteriaQuery = criteriaBuilder.createQuery(GroupPermissionFullDto.class);
        Root<GroupPermission> root = criteriaQuery.from(GroupPermission.class);

        criteriaQuery.multiselect(
                root.get("id"),
                root.get("group").get("id"),
                root.get("permissionId")
        ).where(
                criteriaBuilder.equal(root.get("permissionId"), permissionId)
        );

        return getSession().createQuery(criteriaQuery)
                .setComment(
                        concatenate("GroupPermission with permissionId =", permissionId.toString())
                )
                .getResultList();
    }

    @Override
    @Transactional
    public List<GroupPermissionFullDto> getAllGroupPermissionsByGroupIdAndPermissionId(UUID groupId, UUID permissionId) {

        Assert.notNull(groupId, "groupId must not be null");
        Assert.notNull(permissionId, "permissionId must not be null");

        CriteriaBuilder criteriaBuilder = getSession().getCriteriaBuilder();
        CriteriaQuery<GroupPermissionFullDto> criteriaQuery = criteriaBuilder.createQuery(GroupPermissionFullDto.class);
        Root<GroupPermission> root = criteriaQuery.from(GroupPermission.class);

        criteriaQuery.multiselect(
                root.get("id"),
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
