package groups.schedule.repository;

import groups.groupWorkout.entity.GroupWorkout;
import groups.schedule.dto.ScheduleCellHolder;
import groups.schedule.dto.ScheduleCellHolderFactory;
import org.hibernate.HibernateException;
import org.hibernate.Session;
import org.hibernate.SessionFactory;
import org.hibernate.criterion.Order;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Primary;
import org.springframework.stereotype.Repository;
import org.springframework.util.Assert;

import java.util.List;
import java.util.UUID;

@Primary
@Repository
public class ScheduleHibernateRepository implements ScheduleRepository {

    private final SessionFactory sessionFactory;
    private final ScheduleCellHolderFactory scheduleCellHolderFactory;


    @Autowired
    ScheduleHibernateRepository(SessionFactory sessionFactory) {

        Assert.notNull(sessionFactory, "sessionFactory must not be null");

        this.sessionFactory = sessionFactory;
        this.scheduleCellHolderFactory = new ScheduleCellHolderFactory();
    }


    public ScheduleCellHolder getById(UUID id) {

        Assert.notNull(id, "id must not be null");

        return scheduleCellHolderFactory.create(
                getSession().get(GroupWorkout.class, id)
        );
    }

    public List<ScheduleCellHolder> getAll() {

        return getSession().createCriteria(GroupWorkout.class)
                .addOrder(Order.asc("id"))
                .list()
                .stream()
                .map(groupWorkout -> scheduleCellHolderFactory.create(
                        (GroupWorkout) groupWorkout))
                .toList();
    }

    private Session getSession(){

        try {

            return sessionFactory.getCurrentSession();
        }
        catch (HibernateException e){

            return sessionFactory.openSession();
        }
    }
}