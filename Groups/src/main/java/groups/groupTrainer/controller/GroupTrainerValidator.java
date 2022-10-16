package groups.groupTrainer.controller;

import groups.group.repository.GroupQuery;
import groups.groupTrainer.repository.GroupTrainerQuery;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Component;
import org.springframework.util.Assert;

import java.util.UUID;

@Component
public class GroupTrainerValidator {

    private final GroupQuery groupQuery;
    private final GroupTrainerQuery groupTrainerQuery;


    @Autowired
    public GroupTrainerValidator(GroupQuery groupQuery, GroupTrainerQuery groupTrainerQuery) {

        Assert.notNull(groupQuery, "groupQuery must not be null");
        Assert.notNull(groupTrainerQuery, "groupTrainerQuery must not be null");

        this.groupQuery = groupQuery;
        this.groupTrainerQuery = groupTrainerQuery;
    }


    boolean isCorrectToAssign(GroupTrainerForm groupTrainerForm) {

        return doesTrainerIdExist(groupTrainerForm.trainerId())
                && doesGroupIdExist(groupTrainerForm.groupId())
                && !isAssigned(groupTrainerForm.trainerId(), groupTrainerForm.groupId());
    }

    boolean isCorrectToUnassign(GroupTrainerForm groupTrainerForm) {

        return isAssigned(groupTrainerForm.trainerId(), groupTrainerForm.groupId());
    }

    boolean isCorrectToUnassign(UUID id) {

        return doesGroupTrainerIdExist(id);
    }


    // TODO integracja z innym serwisem
    private boolean doesTrainerIdExist(UUID id) {

        return true;
    }

    private boolean doesGroupIdExist(UUID id) {

        return groupQuery.findGroupById(id).isPresent();
    }

    private boolean doesGroupTrainerIdExist(UUID id) {

        return groupTrainerQuery.findGroupTrainerById(id).isPresent();
    }

    private boolean isAssigned(UUID trainerId, UUID groupId) {

        return !groupTrainerQuery.getAllGroupTrainerByGroupIdAndTrainerId(groupId, trainerId).isEmpty();
    }
}
