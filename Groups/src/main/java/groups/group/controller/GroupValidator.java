package groups.group.controller;

import groups.group.entity.GroupFullDto;
import groups.group.repository.GroupQuery;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Component;
import org.springframework.util.Assert;

@Component
public class GroupValidator {

    private final GroupQuery groupQuery;


    @Autowired
    public GroupValidator(GroupQuery groupQuery) {

        Assert.notNull(groupQuery, "groupQuery must not be null");

        this.groupQuery = groupQuery;
    }


    boolean isCorrectToSave(GroupFullDto groupFullDto) {

        return !doesIdExist(groupFullDto.getId());
    }

    boolean isCorrectToUpdate(GroupFullDto groupFullDto) {

        return doesIdExist(groupFullDto.getId());
    }

    boolean isCorrectToDelete(Long id) {

        return groupQuery.findGroupById(id).isPresent();
    }

    private boolean doesIdExist(Long id) {

        return groupQuery.findGroupById(id).isPresent();
    }
}
