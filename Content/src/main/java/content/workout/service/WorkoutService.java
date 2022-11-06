package content.workout.service;

import content.bodyPart.entity.BodyPart;
import content.bodyPart.entity.BodyPartFactory;
import content.bodyPart.repository.BodyPartRepository;
import content.workout.controller.form.WorkoutForm;
import content.workout.entity.Workout;
import content.workout.entity.WorkoutFactory;
import content.workout.repository.WorkoutRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.util.Assert;

import java.util.UUID;

@Service
public class WorkoutService {

    private final WorkoutRepository workoutRepository;
    private final WorkoutFactory workoutFactory;
    private final BodyPartRepository bodyPartRepository;
    private final BodyPartFactory bodyPartFactory;


    @Autowired
    public WorkoutService(WorkoutRepository workoutRepository, BodyPartRepository bodyPartRepository) {

        Assert.notNull(workoutRepository, "workoutRepository must not be null");
        Assert.notNull(bodyPartRepository, "bodyPartRepository must not be null");

        this.workoutRepository = workoutRepository;
        this.bodyPartRepository = bodyPartRepository;
        this.workoutFactory = new WorkoutFactory();
        this.bodyPartFactory = new BodyPartFactory();
    }


    public UUID saveWorkout(WorkoutForm workoutForm) {

        Assert.notNull(workoutForm, "workoutForm must not be null");

        Workout workout = workoutFactory.create(workoutForm);

        return workoutRepository.save(workout);
    }

    public UUID updateWorkout(UUID id, WorkoutForm workoutForm) {

        Assert.notNull(workoutForm, "workoutForm must not be null");

        Workout workout = workoutRepository.getById(id);

        workout.update(
                workoutForm.description(),
                workoutForm.videoUrl(),
                workoutForm.expectedPerformTime()
        );

        return workoutRepository.update(workout);
    }

    public void deleteWorkout(UUID id) {

        Assert.notNull(id, "id must not be null");

        workoutRepository.delete(id);
    }

    public UUID addBodyPart(UUID workoutId, UUID bodyPartId) {

        Assert.notNull(workoutId, "workoutId must not be null");
        Assert.notNull(bodyPartId, "bodyPartId must not be null");

        Workout workout = workoutRepository.getById(workoutId);
        BodyPart bodyPart = bodyPartRepository.getById(bodyPartId);

        workout.addBodyPart(bodyPart);

        return workoutRepository.update(workout);
    }

    public UUID removeBodyPart(UUID workoutId, UUID bodyPartId) {

        Assert.notNull(workoutId, "workoutId must not be null");
        Assert.notNull(bodyPartId, "bodyPartId must not be null");

        Workout workout = workoutRepository.getById(workoutId);
        BodyPart bodyPart = bodyPartRepository.getById(bodyPartId);

        workout.removeBodyPart(bodyPart);

        return workoutRepository.update(workout);
    }
}
