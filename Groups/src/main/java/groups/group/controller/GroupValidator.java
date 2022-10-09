package groups.group.controller;

import groups.group.entity.GroupFullDto;

//  TODO dodać logikę
public class GroupValidator {

    boolean isCorrectToSave(GroupFullDto groupFullDto) {

        return isCorrect(groupFullDto);
    }

    boolean isCorrectToUpdate(GroupFullDto groupFullDto) {

        return isCorrect(groupFullDto);
    }

    boolean isCorrect(GroupFullDto groupFullDto) {

        return true;
    }
}
