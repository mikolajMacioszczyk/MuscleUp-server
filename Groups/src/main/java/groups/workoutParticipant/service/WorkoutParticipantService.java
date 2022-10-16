package groups.workoutParticipant.service;

import groups.workoutParticipant.controller.WorkoutParticipantForm;
import groups.workoutParticipant.entity.WorkoutParticipant;
import groups.workoutParticipant.entity.WorkoutParticipantFactory;
import groups.workoutParticipant.repository.WorkoutParticipantRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.util.Assert;

import java.util.UUID;

public class WorkoutParticipantService {

    private final WorkoutParticipantRepository workoutParticipantRepository;
    private final WorkoutParticipantFactory workoutParticipantFactory;


    @Autowired
    private WorkoutParticipantService(WorkoutParticipantRepository workoutParticipantRepository, WorkoutParticipantFactory workoutParticipantFactory) {

        Assert.notNull(workoutParticipantRepository, "workoutParticipantRepository must not be null");
        Assert.notNull(workoutParticipantFactory, "workoutParticipantFactory must not be null");

        this.workoutParticipantRepository = workoutParticipantRepository;
        this.workoutParticipantFactory = workoutParticipantFactory;
    }


    public UUID assign(WorkoutParticipantForm workoutParticipantForm) {

        Assert.notNull(workoutParticipantForm, "workoutParticipantForm must not be null");

        WorkoutParticipant workoutParticipant = workoutParticipantFactory.create(workoutParticipantForm);

        return workoutParticipantRepository.assign(workoutParticipant);
    }

    public void unassign(UUID workoutParticipantId) {

        Assert.notNull(workoutParticipantId, "workoutParticipantId must not be null");

        workoutParticipantRepository.unassign(workoutParticipantId);
    }

    public void unassign(WorkoutParticipantForm workoutParticipantForm) {

        Assert.notNull(workoutParticipantForm, "workoutParticipantForm must not be null");

        workoutParticipantRepository.unassign(workoutParticipantForm.groupWorkoutId(), workoutParticipantForm.participantId());
    }
}