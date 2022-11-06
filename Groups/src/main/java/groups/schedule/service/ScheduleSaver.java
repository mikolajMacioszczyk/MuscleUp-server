package groups.schedule.service;

import groups.group.controller.form.GroupForm;
import groups.group.service.GroupService;
import groups.groupTrainer.controller.form.GroupTrainerForm;
import groups.groupTrainer.service.GroupTrainerService;
import groups.groupWorkout.controller.form.GroupWorkoutForm;
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
    private final GroupWorkoutService groupWorkoutService;
    private final GroupTrainerService groupTrainerService;


    @Autowired
    public ScheduleSaver(GroupService groupService,
                               GroupWorkoutService groupWorkoutService,
                               GroupTrainerService groupTrainerService) {

        Assert.notNull(groupService, "groupService must not be null");
        Assert.notNull(groupWorkoutService, "groupWorkoutService must not be null");
        Assert.notNull(groupTrainerService, "groupTrainerService must not be null");

        this.groupService = groupService;
        this.groupWorkoutService = groupWorkoutService;
        this.groupTrainerService = groupTrainerService;
    }


    public UUID save(ScheduleCellForm scheduleCellForm) {

        UUID groupId = createGroup(scheduleCellForm);
        assignTrainer(groupId, scheduleCellForm.trainerId());

        return scheduleCellForm.repeatable()?
                createRepeatableGroupWorkouts(groupId, scheduleCellForm) :
                createSingleGroupWorkout(groupId, scheduleCellForm);
    }


    private UUID createGroup(ScheduleCellForm scheduleCellForm) {

        GroupForm groupForm = new GroupForm(
                scheduleCellForm.name(),
                scheduleCellForm.description(),
                scheduleCellForm.repeatable()
        );

        return groupService.saveGroup(groupForm);
    }

    private UUID createSingleGroupWorkout(UUID groupId, ScheduleCellForm scheduleCellForm) {

        GroupWorkoutForm groupWorkoutForm = new GroupWorkoutForm(
                groupId,
                scheduleCellForm.workoutId(),
                scheduleCellForm.location(),
                scheduleCellForm.maxParticipants(),
                scheduleCellForm.startTime(),
                scheduleCellForm.endTime()
        );

        return groupWorkoutService.saveGroupWorkout(groupWorkoutForm);
    }

    private UUID createRepeatableGroupWorkouts(UUID groupId, ScheduleCellForm scheduleCellForm) {

        UUID parentId = createSingleGroupWorkout(groupId, scheduleCellForm);

        for (long i=1; i<FUTURE_CREATIONS; i++) {

            GroupWorkoutForm groupWorkoutForm = new GroupWorkoutForm(
                    groupId,
                    scheduleCellForm.workoutId(),
                    scheduleCellForm.location(),
                    scheduleCellForm.maxParticipants(),
                    scheduleCellForm.startTime().plusWeeks(i),
                    scheduleCellForm.endTime().plusWeeks(i)
            );

            groupWorkoutService.saveGroupWorkout(groupWorkoutForm, parentId);
        }

        return parentId;
    }

    private void assignTrainer(UUID trainerId, UUID groupId) {

        GroupTrainerForm groupTrainerForm = new GroupTrainerForm(
                groupId,
                trainerId
        );

        groupTrainerService.assign(groupTrainerForm);
    }
}
