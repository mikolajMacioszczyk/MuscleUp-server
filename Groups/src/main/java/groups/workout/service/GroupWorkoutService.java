package groups.workout.service;

import groups.common.UuidWrapper;
import groups.group.repository.GroupRepository;
import groups.workout.controller.GroupWorkoutFullForm;
import groups.workout.entity.GroupWorkout;
import groups.workout.entity.GroupWorkoutFactory;
import groups.workout.entity.GroupWorkoutFullDto;
import groups.workout.repository.GroupWorkoutRepository;
import groups.workoutParticipant.entity.WorkoutParticipant;
import groups.workoutParticipant.service.WorkoutParticipantService;
import groups.workoutPermission.service.WorkoutPermissionService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.util.Assert;

import java.util.List;
import java.util.UUID;
import java.util.stream.Collectors;

@Service
public class GroupWorkoutService {

    private final GroupWorkoutRepository groupWorkoutRepository;
    private final GroupWorkoutFactory groupWorkoutFactory;
    private final GroupRepository groupRepository;
    private final WorkoutParticipantService workoutParticipantService;
    private final WorkoutPermissionService workoutPermissionService;


    @Autowired
    private GroupWorkoutService(GroupWorkoutRepository groupWorkoutRepository,
                                GroupWorkoutFactory groupWorkoutFactory,
                                GroupRepository groupRepository,
                                WorkoutParticipantService workoutParticipantService,
                                WorkoutPermissionService workoutPermissionService) {

        Assert.notNull(groupWorkoutRepository, "groupWorkoutRepository must not be null");
        Assert.notNull(groupWorkoutFactory, "groupWorkoutFactory must not be null");
        Assert.notNull(groupRepository, "groupRepository must not be null");
        Assert.notNull(workoutParticipantService, "workoutParticipantService must not be null");
        Assert.notNull(workoutPermissionService, "workoutPermissionService must not be null");

        this.groupWorkoutRepository = groupWorkoutRepository;
        this.groupWorkoutFactory = groupWorkoutFactory;
        this.groupRepository = groupRepository;
        this.workoutParticipantService = workoutParticipantService;
        this.workoutPermissionService = workoutPermissionService;
    }


    public UUID updateGroupWorkout(GroupWorkoutFullDto groupWorkoutFullDto) {

        Assert.notNull(groupWorkoutFullDto, "groupWorkoutFullDto must not be null");

        GroupWorkout groupWorkout = groupWorkoutRepository.getById(groupWorkoutFullDto.id());

        groupWorkout.update(
                groupWorkoutFullDto.startTime(),
                groupWorkoutFullDto.endTime(),
                groupRepository.getById(groupWorkoutFullDto.groupId()),
                groupWorkoutFullDto.workoutId()
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
        workoutPermissionService.unassignAllByGroupWorkoutId(idToRemove);
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