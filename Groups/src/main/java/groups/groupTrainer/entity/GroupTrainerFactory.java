package groups.groupTrainer.entity;

import groups.group.repository.GroupQuery;
import groups.groupTrainer.controller.form.GroupTrainerForm;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Component;
import org.springframework.util.Assert;

@Component
public class GroupTrainerFactory {

    private final GroupQuery groupQuery;


    @Autowired
    private GroupTrainerFactory(GroupQuery groupQuery) {

        Assert.notNull(groupQuery, "groupQuery must not be null");

        this.groupQuery = groupQuery;
    }


    public GroupTrainer create(GroupTrainerForm groupTrainerForm) {

        Assert.notNull(groupTrainerForm, "groupTrainerForm must not be null");

        return new GroupTrainer(
                groupQuery.getById(groupTrainerForm.groupId()),
                groupTrainerForm.trainerId()
        );
    }
}
