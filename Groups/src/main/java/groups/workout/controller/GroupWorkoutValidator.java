package groups.workout.controller;

import groups.group.repository.GroupQuery;
import groups.workout.entity.GroupWorkoutFullDto;
import groups.workout.repository.GroupWorkoutQuery;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Component;
import org.springframework.util.Assert;

import java.time.LocalDateTime;
import java.util.UUID;

@Component
public class GroupWorkoutValidator {

    private final GroupWorkoutQuery groupWorkoutQuery;
    private final GroupQuery groupQuery;


    @Autowired
    public GroupWorkoutValidator(GroupWorkoutQuery groupWorkoutQuery, GroupQuery groupQuery) {

        Assert.notNull(groupWorkoutQuery, "groupWorkoutQuery must not be null");
        Assert.notNull(groupQuery, "groupQuery must not be null");

        this.groupWorkoutQuery = groupWorkoutQuery;
        this.groupQuery = groupQuery;
    }


    // TODO czy nie ma groupWorkout z workoutId i groupId w tych godzinach
    boolean isCorrectToSave(GroupWorkoutFullForm groupWorkoutFullForm) {

        return doesGroupIdExist(groupWorkoutFullForm.groupId())
                && doesWorkoutIdExist(groupWorkoutFullForm.workoutId())
                && areDatesCorrect(groupWorkoutFullForm.startTime(), groupWorkoutFullForm.endTime());
    }

    boolean isCorrectToUpdate(GroupWorkoutFullDto groupWorkoutFullDto) {

        return doesGroupWorkoutIdExist(groupWorkoutFullDto.id())
                && doesGroupIdExist(groupWorkoutFullDto.groupId())
                && doesWorkoutIdExist(groupWorkoutFullDto.workoutId())
                && areDatesCorrect(groupWorkoutFullDto.startTime(), groupWorkoutFullDto.endTime());
    }

    boolean isCorrectToDelete(UUID id) {

        return groupWorkoutQuery.findGroupWorkoutById(id).isPresent();
    }


    private boolean doesGroupWorkoutIdExist(UUID id) {

        return groupWorkoutQuery.findGroupWorkoutById(id).isPresent();
    }

    private boolean doesGroupIdExist(UUID id) {

        return groupQuery.findGroupById(id).isPresent();
    }

    // TODO integracja z innym serwisem
    private boolean doesWorkoutIdExist(UUID id) {

        return true;
    }

    private boolean areDatesCorrect(LocalDateTime dateFrom, LocalDateTime dateTo) {

        return dateFrom.isBefore(dateTo);
    }
}
