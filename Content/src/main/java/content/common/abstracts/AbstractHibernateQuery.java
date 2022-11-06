package content.common.abstracts;

import org.hibernate.HibernateException;
import org.hibernate.Session;
import org.hibernate.SessionFactory;
import org.hibernate.criterion.Order;
import org.springframework.stereotype.Repository;
import org.springframework.util.Assert;

import java.util.List;
import java.util.UUID;

@Repository
public abstract class AbstractHibernateQuery<T extends AbstractEntity> {

    private final Class<T> clazz;
    protected final SessionFactory sessionFactory;


    protected AbstractHibernateQuery(Class<T> clazz, SessionFactory sessionFactory) {

        Assert.notNull(clazz, "clazz must not be null");
        Assert.notNull(sessionFactory, "sessionFactory must not be null");

        this.clazz = clazz;
        this.sessionFactory = sessionFactory;
    }


    public List<T> getAll() {

        return getSession().createCriteria(clazz)
                .addOrder(Order.asc("id"))
                .list();
    }

    public T getById(UUID id) {

        Assert.notNull(id, "id must not be null");

        return getSession().get(clazz, id);
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
