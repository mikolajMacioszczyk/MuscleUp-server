package groups.workoutParticipant.controller;

import groups.workout.repository.GroupWorkoutQuery;
import groups.workoutParticipant.repository.WorkoutParticipantQuery;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Component;
import org.springframework.util.Assert;

import java.util.UUID;

@Component
public class WorkoutParticipantValidator {

    private final GroupWorkoutQuery groupWorkoutQuery;
    private final WorkoutParticipantQuery workoutParticipantQuery;


    @Autowired
    private WorkoutParticipantValidator(GroupWorkoutQuery groupWorkoutQuery, WorkoutParticipantQuery workoutParticipantQuery) {

        Assert.notNull(groupWorkoutQuery, "groupWorkoutQuery must not be null");
        Assert.notNull(workoutParticipantQuery, "workoutParticipantQuery must not be null");

        this.groupWorkoutQuery = groupWorkoutQuery;
        this.workoutParticipantQuery = workoutParticipantQuery;
    }


    boolean isCorrectToAssign(WorkoutParticipantForm workoutParticipantForm) {

        Assert.notNull(workoutParticipantForm, "workoutParticipantForm must not be null");

        return doesGroupWorkoutIdExist(workoutParticipantForm.groupWorkoutId())
                && doesParticipantExist(workoutParticipantForm.participantId())
                && !isAssigned(workoutParticipantForm.groupWorkoutId(), workoutParticipantForm.participantId());
    }

    boolean isCorrectToUnassign(WorkoutParticipantForm workoutParticipantForm) {

        Assert.notNull(workoutParticipantForm, "workoutParticipantForm must not be null");

        return isAssigned(workoutParticipantForm.groupWorkoutId(), workoutParticipantForm.participantId());
    }

    boolean isCorrectToUnassign(UUID id) {

        Assert.notNull(id, "id must not be null");

        return doesGroupParticipantExist(id);
    }


    // TODO integracja z innym serwisem
    private boolean doesParticipantExist(UUID participantId) {

        return true;
    }

    private boolean doesGroupWorkoutIdExist(UUID groupWorkoutId) {

        return groupWorkoutQuery.findGroupWorkoutById(groupWorkoutId).isPresent();
    }

    private boolean doesGroupParticipantExist(UUID id) {

        return workoutParticipantQuery.findWorkoutParticipantById(id).isPresent();
    }

    private boolean isAssigned(UUID groupWorkoutId, UUID participantId) {

        return !workoutParticipantQuery.getAllWorkoutParticipantsByGroupWorkoutIdAndParticipantId(groupWorkoutId, participantId).isEmpty();
    }
}
