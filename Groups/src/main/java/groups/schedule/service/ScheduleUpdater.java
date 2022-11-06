package groups.schedule.service;

import groups.group.controller.form.GroupForm;
import groups.group.service.GroupService;
import groups.groupPermission.controller.form.GroupPermissionForm;
import groups.groupPermission.service.GroupPermissionService;
import groups.groupTrainer.controller.form.GroupTrainerForm;
import groups.groupTrainer.service.GroupTrainerService;
import groups.groupWorkout.controller.form.GroupWorkoutForm;
import groups.groupWorkout.repository.GroupWorkoutQuery;
import groups.groupWorkout.service.GroupWorkoutService;
import groups.schedule.controller.form.ScheduleCellForm;
import groups.schedule.dto.ScheduleCell;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.util.Assert;

import java.util.List;
import java.util.UUID;

import static java.util.Objects.isNull;

@Service
public class ScheduleUpdater {

    private final GroupService groupService;
    private final GroupWorkoutService groupWorkoutService;
    private final GroupWorkoutQuery groupWorkoutQuery;
    private final GroupPermissionService groupPermissionService;
    private final GroupTrainerService groupTrainerService;
    private final ScheduleListService scheduleListService;


    @Autowired
    public ScheduleUpdater(GroupService groupService,
                           GroupWorkoutService groupWorkoutService,
                           GroupWorkoutQuery groupWorkoutQuery,
                           GroupPermissionService groupPermissionService,
                           GroupTrainerService groupTrainerService,
                           ScheduleListService scheduleListService) {

        Assert.notNull(groupService, "groupService must not be null");
        Assert.notNull(groupWorkoutService, "groupWorkoutService must not be null");
        Assert.notNull(groupWorkoutQuery, "groupWorkoutQuery must not be null");
        Assert.notNull(groupPermissionService, "groupPermissionService must not be null");
        Assert.notNull(groupTrainerService, "groupTrainerService must not be null");
        Assert.notNull(scheduleListService, "scheduleListService must not be null");

        this.groupService = groupService;
        this.groupWorkoutService = groupWorkoutService;
        this.groupWorkoutQuery = groupWorkoutQuery;
        this.groupPermissionService = groupPermissionService;
        this.groupTrainerService = groupTrainerService;
        this.scheduleListService = scheduleListService;
    }


    public UUID update(UUID id, ScheduleCellForm form, boolean cascade) {

        ScheduleCell currentCell = scheduleListService.composeCell(id);

        if (form.hasGroupChanged(currentCell)) {

            UUID newGroupId = createNewGroupAndReassign(currentCell, form);
            groupWorkoutService.removeParent(id);

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

        if (form.hasTrainerChanged(currentCell)) {

            assignTrainer(form.trainerId(), newGroupId);
        }
        else {

            assignTrainer(currentCell.getTrainer().trainerId(), newGroupId);
        }

        reassignPermissions(currentCell.getPermissions(), newGroupId);

        return newGroupId;
    }

    private UUID createGroup(ScheduleCellForm form) {

        GroupForm groupForm = new GroupForm(
                form.name(),
                form.description(),
                form.repeatable()
        );

        return groupService.saveGroup(groupForm);
    }

    private UUID singleUpdateGroupWorkout(UUID groupWorkoutId, UUID groupId, ScheduleCellForm form) {

        GroupWorkoutForm groupWorkoutForm = new GroupWorkoutForm(
                groupId,
                form.workoutId(),
                form.location(),
                form.maxParticipants(),
                form.startTime(),
                form.endTime()
        );

        return groupWorkoutService.updateGroupWorkout(groupWorkoutId, groupWorkoutForm);
    }

    // TODO add subquery to reduce database connections
    private UUID cascadeUpdateGroupWorkout(UUID groupWorkoutId, UUID groupId, ScheduleCellForm form) {

        UUID parentId = groupWorkoutQuery.getParentIdById(groupWorkoutId);

        if (isNull(parentId)) {

            parentId = groupWorkoutId;
        }

        List<UUID> originalWithClones = groupWorkoutQuery.getFutureGroupWorkoutsByParentId(parentId);

        originalWithClones.forEach(id -> {

            GroupWorkoutForm groupWorkoutForm = new GroupWorkoutForm(
                    groupId,
                    form.workoutId(),
                    form.location(),
                    form.maxParticipants(),
                    form.startTime(),
                    form.endTime()
            );

            groupWorkoutService.updateGroupWorkout(id, groupWorkoutForm);
        });

        return groupWorkoutId;
    }

    private void assignTrainer(UUID trainerId, UUID groupId) {

        GroupTrainerForm groupTrainerForm = new GroupTrainerForm(
                trainerId,
                groupId
        );

        groupTrainerService.assign(groupTrainerForm);
    }

    private void reassignPermissions(List<UUID> permissionIds, UUID groupId) {

        permissionIds.forEach( permissionId ->
                groupPermissionService.add(new GroupPermissionForm(groupId, permissionId))
        );
    }
}
