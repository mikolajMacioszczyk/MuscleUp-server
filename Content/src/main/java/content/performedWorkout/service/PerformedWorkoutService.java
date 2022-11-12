package content.performedWorkout.service;

import content.performedWorkout.controller.form.PerformedWorkoutForm;
import content.performedWorkout.entity.PerformedWorkout;
import content.performedWorkout.entity.PerformedWorkoutFactory;
import content.performedWorkout.repository.PerformedWorkoutRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.util.Assert;

import java.util.UUID;

@Service
public class PerformedWorkoutService {

    private final PerformedWorkoutRepository performedWorkoutRepository;
    private final PerformedWorkoutFactory performedWorkoutFactory;


    @Autowired
    public PerformedWorkoutService(PerformedWorkoutRepository performedWorkoutRepository,
                                   PerformedWorkoutFactory performedWorkoutFactory) {

        Assert.notNull(performedWorkoutRepository, "performedWorkoutRepository must not be null");
        Assert.notNull(performedWorkoutFactory, "performedWorkoutFactory must not be null");

        this.performedWorkoutRepository = performedWorkoutRepository;
        this.performedWorkoutFactory = performedWorkoutFactory;
    }


    public UUID savePerformedWorkout(PerformedWorkoutForm form) {

        Assert.notNull(form, "form must not be null");

        PerformedWorkout performedWorkout = performedWorkoutFactory.create(form);

        return performedWorkoutRepository.save(performedWorkout);
    }
}
