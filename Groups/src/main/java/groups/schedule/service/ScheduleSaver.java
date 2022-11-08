package groups.schedule.service;

import groups.group.controller.form.GroupForm;
import groups.group.service.GroupService;
import groups.groupWorkout.controller.form.GroupWorkoutForm;
import groups.groupWorkout.repository.GroupWorkoutQuery;
import groups.groupWorkout.service.GroupWorkoutService;
import groups.schedule.controller.form.ScheduleCellForm;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.util.Assert;

import java.util.UUID;

import static groups.common.utils.EnvironmentUtils.getFutureCreations;

@Service
public class ScheduleSaver {

    private static final int FUTURE_CREATIONS = getFutureCreations();

    private final GroupService groupService;
    private final GroupWorkoutQuery groupWorkoutQuery;
    private final GroupWorkoutService groupWorkoutService;


    @Autowired
    public ScheduleSaver(GroupService groupService, GroupWorkoutQuery groupWorkoutQuery, GroupWorkoutService groupWorkoutService) {

        Assert.notNull(groupService, "groupService must not be null");
        Assert.notNull(groupWorkoutQuery, "groupWorkoutQuery must not be null");
        Assert.notNull(groupWorkoutService, "groupWorkoutService must not be null");

        this.groupService = groupService;
        this.groupWorkoutQuery = groupWorkoutQuery;
        this.groupWorkoutService = groupWorkoutService;
    }


    public UUID save(ScheduleCellForm scheduleCellForm) {

        UUID groupId = createGroup(scheduleCellForm);

        return scheduleCellForm.repeatable()?
                createRepeatableGroupWorkouts(groupId, scheduleCellForm) :
                createSingleGroupWorkout(groupId, scheduleCellForm);
    }


    private UUID createGroup(ScheduleCellForm scheduleCellForm) {

        GroupForm groupForm = new GroupForm(
                scheduleCellForm.name(),
                scheduleCellForm.trainerId(),
                scheduleCellForm.fitnessClubId(),
                scheduleCellForm.description(),
                scheduleCellForm.location(),
                scheduleCellForm.maxParticipants(),
                scheduleCellForm.repeatable()
        );

        return groupService.saveGroup(groupForm);
    }

    private UUID createSingleGroupWorkout(UUID groupId, ScheduleCellForm scheduleCellForm) {

        GroupWorkoutForm groupWorkoutForm = new GroupWorkoutForm(
                groupId,
                scheduleCellForm.workoutId(),
                scheduleCellForm.startTime(),
                scheduleCellForm.endTime()
        );

        return groupWorkoutService.saveGroupWorkout(groupWorkoutForm);
    }

    private UUID createRepeatableGroupWorkouts(UUID groupId, ScheduleCellForm scheduleCellForm) {

        UUID createdId = createSingleGroupWorkout(groupId, scheduleCellForm);
        UUID cloneId = groupWorkoutQuery.getCloneIdById(createdId);

        for (long i=1; i<FUTURE_CREATIONS; i++) {

            GroupWorkoutForm groupWorkoutForm = new GroupWorkoutForm(
                    groupId,
                    scheduleCellForm.workoutId(),
                    scheduleCellForm.startTime().plusWeeks(i),
                    scheduleCellForm.endTime().plusWeeks(i)
            );

            groupWorkoutService.saveGroupWorkout(groupWorkoutForm, cloneId);
        }

        return cloneId;
    }
}
