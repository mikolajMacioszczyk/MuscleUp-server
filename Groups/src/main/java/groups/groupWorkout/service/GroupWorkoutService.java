package groups.groupWorkout.service;

import groups.common.wrappers.UuidWrapper;
import groups.group.repository.GroupRepository;
import groups.groupWorkout.controller.form.GroupWorkoutFullForm;
import groups.groupWorkout.entity.GroupWorkout;
import groups.groupWorkout.entity.GroupWorkoutFactory;
import groups.groupWorkout.entity.GroupWorkoutFullDto;
import groups.groupWorkout.repository.GroupWorkoutRepository;
import groups.workoutParticipant.service.WorkoutParticipantService;
import groups.workoutPermission.service.GroupPermissionService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.util.Assert;

import java.util.List;
import java.util.UUID;

@Service
public class GroupWorkoutService {

    private final GroupWorkoutRepository groupWorkoutRepository;
    private final GroupWorkoutFactory groupWorkoutFactory;
    private final GroupRepository groupRepository;
    private final WorkoutParticipantService workoutParticipantService;
    private final GroupPermissionService groupPermissionService;


    @Autowired
    private GroupWorkoutService(GroupWorkoutRepository groupWorkoutRepository,
                                GroupWorkoutFactory groupWorkoutFactory,
                                GroupRepository groupRepository,
                                WorkoutParticipantService workoutParticipantService,
                                GroupPermissionService groupPermissionService) {

        Assert.notNull(groupWorkoutRepository, "groupWorkoutRepository must not be null");
        Assert.notNull(groupWorkoutFactory, "groupWorkoutFactory must not be null");
        Assert.notNull(groupRepository, "groupRepository must not be null");
        Assert.notNull(workoutParticipantService, "workoutParticipantService must not be null");
        Assert.notNull(groupPermissionService, "workoutPermissionService must not be null");

        this.groupWorkoutRepository = groupWorkoutRepository;
        this.groupWorkoutFactory = groupWorkoutFactory;
        this.groupRepository = groupRepository;
        this.workoutParticipantService = workoutParticipantService;
        this.groupPermissionService = groupPermissionService;
    }


    public UUID updateGroupWorkout(GroupWorkoutFullDto groupWorkoutFullDto) {

        Assert.notNull(groupWorkoutFullDto, "groupWorkoutFullDto must not be null");

        GroupWorkout groupWorkout = groupWorkoutRepository.getById(groupWorkoutFullDto.id());

        groupWorkout.update(
                groupRepository.getById(groupWorkoutFullDto.groupId()),
                groupWorkoutFullDto.workoutId(),
                groupWorkout.getLocation(),
                groupWorkout.getMaxParticipants()
        );

        return groupWorkoutRepository.update(groupWorkout);
    }

    public UUID saveGroupWorkout(GroupWorkoutFullForm groupWorkoutFullForm) {

        Assert.notNull(groupWorkoutFullForm, "groupWorkoutFullForm must not be null");

        GroupWorkout groupWorkout = groupWorkoutFactory.create(groupWorkoutFullForm);

        return groupWorkoutRepository.save(groupWorkout);
    }

    public void deleteGroupWorkout(UUID idToRemove) {

        Assert.notNull(idToRemove, "idToRemove must not be null");

        workoutParticipantService.unassignAllByGroupWorkoutId(idToRemove);
        groupPermissionService.unassignAllByGroupWorkoutId(idToRemove);
        groupWorkoutRepository.delete(idToRemove);
    }

    public void deleteAllByGroupId(UUID groupIdToRemove) {

        Assert.notNull(groupIdToRemove, "groupIdToRemove must not be null");

        List<UUID> groupWorkoutIdsToRemove = groupWorkoutRepository.getIdsByGroupId(groupIdToRemove)
                .stream()
                .map(UuidWrapper::uuid)
                .toList();

        groupWorkoutIdsToRemove.forEach(this::deleteGroupWorkout);
    }
}