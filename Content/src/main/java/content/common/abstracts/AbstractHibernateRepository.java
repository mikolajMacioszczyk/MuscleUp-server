package content.common.abstracts;

import org.hibernate.HibernateException;
import org.hibernate.Session;
import org.hibernate.SessionFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Repository;
import org.springframework.util.Assert;

import javax.transaction.Transactional;
import java.util.UUID;

@Repository
public abstract class AbstractHibernateRepository<T extends AbstractEntity> {

    private final Class<T> clazz;
    protected final SessionFactory sessionFactory;


    @Autowired
    protected AbstractHibernateRepository(Class<T> clazz, SessionFactory sessionFactory) {

        Assert.notNull(clazz, "clazz must not be null");
        Assert.notNull(sessionFactory, "sessionFactory must not be null");

        this.clazz = clazz;
        this.sessionFactory = sessionFactory;
    }


    public T getById(UUID id) {

        Assert.notNull(id, "id must not be null");

        return getSession().get(clazz, id);
    }

    @Transactional
    public UUID save(T entity) {

        Assert.notNull(entity, "entity must not be null");

        Session session = getSession();

        session.save(clazz.getName(), entity);

        return entity.getId();
    }

    @Transactional
    public UUID update(T entity) {

        Assert.notNull(entity, "entity must not be null");

        Session session = getSession();

        session.update(clazz.getName(), entity);

        return entity.getId();
    }

    @Transactional
    public void delete(UUID id) {

        Assert.notNull(id, "id must not be null");

        Session session = getSession();

        session.delete(session.get(clazz, id));
    }

    protected Session getSession(){

        try {

            return sessionFactory.getCurrentSession();
        }
        catch (HibernateException e){

            return sessionFactory.openSession();
        }
    }
}
