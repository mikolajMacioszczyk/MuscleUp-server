package groups.group.controller;

import groups.group.entity.GroupFullDto;
import groups.group.repository.GroupQuery;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Component;
import org.springframework.util.Assert;

import java.util.UUID;

import static groups.common.stringUtils.StringUtils.isNullOrEmpty;

@Component
public class GroupValidator {

    private final static Long MIN_PARTICIPANTS_PER_GROUP = 1L;

    private final GroupQuery groupQuery;


    @Autowired
    public GroupValidator(GroupQuery groupQuery) {

        Assert.notNull(groupQuery, "groupQuery must not be null");

        this.groupQuery = groupQuery;
    }


    boolean isCorrectToSave(GroupFullForm groupFullForm) {

        return isParticipantNumberCorrect(groupFullForm.maxParticipants())
                && isNameCorrect(groupFullForm.name());
    }

    boolean isCorrectToUpdate(GroupFullDto groupFullDto) {

        return doesIdExist(groupFullDto.id())
                && isParticipantNumberCorrect(groupFullDto.maxParticipants())
                && isNameCorrect(groupFullDto.name());
    }

    boolean isCorrectToDelete(UUID id) {

        return groupQuery.findGroupById(id).isPresent();
    }


    private boolean isParticipantNumberCorrect(Long participantNumber) {

        return participantNumber >= MIN_PARTICIPANTS_PER_GROUP;
    }

    private boolean doesIdExist(UUID id) {

        return groupQuery.findGroupById(id).isPresent();
    }

    private boolean isNameCorrect(String name) {

        return !isNullOrEmpty(name);
    }
}
