package groups.workoutParticipant.service;

import groups.workoutParticipant.controller.form.WorkoutParticipantForm;
import groups.workoutParticipant.entity.WorkoutParticipant;
import groups.workoutParticipant.entity.WorkoutParticipantFactory;
import groups.workoutParticipant.repository.WorkoutParticipantRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.util.Assert;

import java.util.UUID;

@Service
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


    public UUID assign(WorkoutParticipantForm form) {

        Assert.notNull(form, "form must not be null");

        WorkoutParticipant workoutParticipant = workoutParticipantFactory.create(
                form.groupWorkoutId(),
                form.userId()
        );

        return workoutParticipantRepository.assign(workoutParticipant);
    }

    public void unassign(WorkoutParticipantForm form) {

        Assert.notNull(form, "form must not be null");

        workoutParticipantRepository.unassign(form.groupWorkoutId(), form.userId());
    }
}