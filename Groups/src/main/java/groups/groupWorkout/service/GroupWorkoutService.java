package groups.groupWorkout.service;

import groups.group.repository.GroupRepository;
import groups.groupWorkout.controller.form.GroupWorkoutForm;
import groups.groupWorkout.entity.GroupWorkout;
import groups.groupWorkout.entity.GroupWorkoutFactory;
import groups.groupWorkout.repository.GroupWorkoutRepository;
import groups.groupWorkout.service.form.GroupWorkoutUpdateForm;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.util.Assert;

import java.util.UUID;

import static groups.common.utils.TimeUtils.addTimeDiff;
import static java.util.UUID.randomUUID;

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

    public UUID saveGroupWorkout(GroupWorkoutForm groupWorkoutForm, UUID cloneId) {

        Assert.notNull(groupWorkoutForm, "groupWorkoutFullForm must not be null");
        Assert.notNull(cloneId, "cloneId must not be null");

        GroupWorkout groupWorkout = groupWorkoutFactory.create(groupWorkoutForm, cloneId);

        return groupWorkoutRepository.save(groupWorkout);
    }

    public UUID updateGroupWorkout(UUID id, GroupWorkoutForm groupWorkoutForm) {

        Assert.notNull(id, "id must not be null");
        Assert.notNull(groupWorkoutForm, "groupWorkoutForm must not be null");

        GroupWorkout groupWorkout = groupWorkoutRepository.getById(id);

        groupWorkout.update(
                groupRepository.getById(groupWorkoutForm.groupId()),
                groupWorkoutForm.workoutId(),
                groupWorkoutForm.startTime(),
                groupWorkoutForm.endTime(),
                randomUUID()
        );

        return groupWorkoutRepository.update(groupWorkout);
    }

    public void updateGroupWorkout(UUID id, GroupWorkoutUpdateForm form) {

        Assert.notNull(id, "id must not be null");
        Assert.notNull(form, "form must not be null");

        GroupWorkout groupWorkout = groupWorkoutRepository.getById(id);

        groupWorkout.update(
                groupRepository.getById(form.groupId()),
                form.workoutId(),
                addTimeDiff(groupWorkout.getStartTime(), form.startTimeDiff()),
                addTimeDiff(groupWorkout.getEndTime(), form.endTimeDiff()),
                form.cloneId()
        );

        groupWorkoutRepository.update(groupWorkout);
    }

    public void deleteGroupWorkout(UUID idToRemove) {

        Assert.notNull(idToRemove, "idToRemove must not be null");

        groupWorkoutRepository.delete(idToRemove);
    }
}
