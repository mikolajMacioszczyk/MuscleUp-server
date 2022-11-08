package groups.schedule.service;

import groups.common.wrappers.TimeWrapper;
import groups.group.controller.form.GroupForm;
import groups.group.service.GroupService;
import groups.groupPermission.service.GroupPermissionService;
import groups.groupWorkout.controller.form.GroupWorkoutForm;
import groups.groupWorkout.repository.GroupWorkoutQuery;
import groups.groupWorkout.service.GroupWorkoutService;
import groups.groupWorkout.service.form.GroupWorkoutUpdateForm;
import groups.schedule.controller.form.ScheduleCellForm;
import groups.schedule.dto.ScheduleCell;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.util.Assert;

import java.util.List;
import java.util.UUID;

import static groups.common.utils.TimeUtils.calculateTimeDiff;
import static java.util.UUID.randomUUID;

@Service
public class ScheduleUpdater {

    private final GroupService groupService;
    private final GroupWorkoutService groupWorkoutService;
    private final GroupWorkoutQuery groupWorkoutQuery;
    private final GroupPermissionService groupPermissionService;
    private final ScheduleListService scheduleListService;


    @Autowired
    public ScheduleUpdater(GroupService groupService,
                           GroupWorkoutService groupWorkoutService,
                           GroupWorkoutQuery groupWorkoutQuery,
                           GroupPermissionService groupPermissionService,
                           ScheduleListService scheduleListService) {

        Assert.notNull(groupService, "groupService must not be null");
        Assert.notNull(groupWorkoutService, "groupWorkoutService must not be null");
        Assert.notNull(groupWorkoutQuery, "groupWorkoutQuery must not be null");
        Assert.notNull(groupPermissionService, "groupPermissionService must not be null");
        Assert.notNull(scheduleListService, "scheduleListService must not be null");

        this.groupService = groupService;
        this.groupWorkoutService = groupWorkoutService;
        this.groupWorkoutQuery = groupWorkoutQuery;
        this.groupPermissionService = groupPermissionService;
        this.scheduleListService = scheduleListService;
    }


    public UUID update(UUID id, ScheduleCellForm form, boolean cascade) {

        ScheduleCell currentCell = scheduleListService.composeCell(id);

        if (form.hasGroupChanged(currentCell)) {

            UUID newGroupId = createNewGroupAndReassign(currentCell, form);

            return cascade?
                    cascadeUpdateGroupWorkout(id, newGroupId, form) :
                    singleUpdateGroupWorkout(id, newGroupId, form);
        }
        else if (form.hasGroupWorkoutChanged(currentCell)) {

            return cascade?
                    cascadeUpdateGroupWorkout(id, currentCell.getGroup().id(), form) :
                    singleUpdateGroupWorkout(id, currentCell.getGroup().id(), form);
        }

        return id;
    }

    private UUID createNewGroupAndReassign(ScheduleCell currentCell, ScheduleCellForm form) {

        UUID newGroupId = createGroup(form);

        reassignPermissions(currentCell.getPermissions(), newGroupId);

        return newGroupId;
    }

    private void reassignPermissions(List<UUID> permissionIds, UUID groupId) {

        permissionIds.forEach( permissionId ->
                groupPermissionService.assign(groupId, permissionId)
        );
    }

    private UUID createGroup(ScheduleCellForm form) {

        GroupForm groupForm = new GroupForm(
                form.name(),
                form.trainerId(),
                form.fitnessClubId(),
                form.description(),
                form.location(),
                form.maxParticipants(),
                form.repeatable()
        );

        return groupService.saveGroup(groupForm);
    }

    private UUID singleUpdateGroupWorkout(UUID groupWorkoutId, UUID groupId, ScheduleCellForm form) {

        GroupWorkoutForm groupWorkoutForm = new GroupWorkoutForm(
                groupId,
                form.workoutId(),
                form.startTime(),
                form.endTime()
        );

        return groupWorkoutService.updateGroupWorkout(groupWorkoutId, groupWorkoutForm);
    }

    private UUID cascadeUpdateGroupWorkout(UUID groupWorkoutId, UUID groupId, ScheduleCellForm form) {

        UUID cloneId = groupWorkoutQuery.getCloneIdById(groupWorkoutId);
        List<UUID> clonesWithOriginal = groupWorkoutQuery.getFutureGroupWorkoutsByCloneId(cloneId);
        UUID newCloneId = randomUUID();

        TimeWrapper originalTime = groupWorkoutQuery.getTimeById(groupWorkoutId);

        clonesWithOriginal.forEach(id -> {

            GroupWorkoutUpdateForm groupWorkoutUpdateForm = new GroupWorkoutUpdateForm(
                    groupId,
                    form.workoutId(),
                    calculateTimeDiff(originalTime.startTime(), form.startTime()),
                    calculateTimeDiff(originalTime.endTime(), form.endTime()),
                    newCloneId
            );

            groupWorkoutService.updateGroupWorkout(id, groupWorkoutUpdateForm);
        });

        return groupWorkoutId;
    }
}
