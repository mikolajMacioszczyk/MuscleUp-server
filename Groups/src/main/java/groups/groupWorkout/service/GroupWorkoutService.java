package groups.groupWorkout.service;

import groups.group.repository.GroupRepository;
import groups.groupWorkout.controller.form.GroupWorkoutForm;
import groups.groupWorkout.entity.GroupWorkout;
import groups.groupWorkout.entity.GroupWorkoutFactory;
import groups.groupWorkout.repository.GroupWorkoutRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.util.Assert;

import java.util.UUID;

@Service
public class GroupWorkoutService {

    private final GroupWorkoutRepository groupWorkoutRepository;
    private final GroupWorkoutFactory groupWorkoutFactory;
    private final GroupRepository groupRepository;


    @Autowired
    private GroupWorkoutService(GroupWorkoutRepository groupWorkoutRepository,
                                GroupWorkoutFactory groupWorkoutFactory,
                                GroupRepository groupRepository) {

        Assert.notNull(groupWorkoutRepository, "groupWorkoutRepository must not be null");
        Assert.notNull(groupWorkoutFactory, "groupWorkoutFactory must not be null");
        Assert.notNull(groupRepository, "groupRepository must not be null");

        this.groupWorkoutRepository = groupWorkoutRepository;
        this.groupWorkoutFactory = groupWorkoutFactory;
        this.groupRepository = groupRepository;
    }


    public UUID saveGroupWorkout(GroupWorkoutForm groupWorkoutForm) {

        Assert.notNull(groupWorkoutForm, "groupWorkoutFullForm must not be null");

        GroupWorkout groupWorkout = groupWorkoutFactory.create(groupWorkoutForm);

        return groupWorkoutRepository.save(groupWorkout);
    }

    public void saveGroupWorkout(GroupWorkoutForm groupWorkoutForm, UUID parentId) {

        Assert.notNull(groupWorkoutForm, "groupWorkoutFullForm must not be null");
        Assert.notNull(parentId, "parentId must not be null");

        GroupWorkout groupWorkout = groupWorkoutFactory.create(groupWorkoutForm, parentId);

        groupWorkoutRepository.save(groupWorkout);
    }

    public UUID updateGroupWorkout(UUID id, GroupWorkoutForm groupWorkoutForm) {

        Assert.notNull(id, "id must not be null");
        Assert.notNull(groupWorkoutForm, "groupWorkoutForm must not be null");

        GroupWorkout groupWorkout = groupWorkoutRepository.getById(id);

        groupWorkout.update(
                groupRepository.getById(groupWorkoutForm.groupId()),
                groupWorkoutForm.workoutId(),
                groupWorkoutForm.location(),
                groupWorkoutForm.maxParticipants(),
                groupWorkoutForm.startTime(),
                groupWorkoutForm.endTime(),
                groupWorkout.getParentId()
        );

        return groupWorkoutRepository.update(groupWorkout);
    }

    public void deleteGroupWorkout(UUID idToRemove) {

        Assert.notNull(idToRemove, "idToRemove must not be null");

        groupWorkoutRepository.delete(idToRemove);
    }

    public void removeParent(UUID id) {

        Assert.notNull(id, "id must not be null");

        GroupWorkout groupWorkout = groupWorkoutRepository.getById(id);

        groupWorkout.update(
                groupWorkout.getGroup(),
                groupWorkout.getWorkoutId(),
                groupWorkout.getLocation(),
                groupWorkout.getMaxParticipants(),
                groupWorkout.getStartTime(),
                groupWorkout.getEndTime(),
                null
        );

        groupWorkoutRepository.update(groupWorkout);
    }
}